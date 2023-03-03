using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Player
{
    [RequireComponent(typeof(PlayerAnimations))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovement : MonoBehaviour
    {
        private PlayerAnimations _animations;
        private Rigidbody2D _playerRigidbody;
        
        private float _moveHorizontal;
        private bool _facingRight = true;
        private float _fallTime;
        
        [SerializeField]
        private float moveSpeed = 3f;

        [SerializeField]
        private Transform[] excludeFromFlip;

        // TODO: Extract camera shake
        [Header("Camera Shake")]
        [SerializeField]
        private float cameraShakeDuration = 1;
        
        [SerializeField]
        private float cameraShakeMagnitude = 1;
        
        [SerializeField]
        private float shakeFallMultiplier = 2;

        private void Awake()
        {
            _animations = GetComponent<PlayerAnimations>();
            _playerRigidbody = GetComponent<Rigidbody2D>();
        }

        public void UpdateMovement(float movement)
        {
            _moveHorizontal = movement;
            _animations.SetMoveSpeed(_moveHorizontal);
        }

        private void FixedUpdate()
        {
            // TODO: Dampen movement when flying and not attached to grapple
            if (_moveHorizontal is > 0f or < 0f)
            {
                _playerRigidbody.AddForce(new Vector2(_moveHorizontal * moveSpeed, 0), ForceMode2D.Impulse);
                
                // TODO: Only shake on the ground
                // TODO: Shake while falling
                var shake = Shake(cameraShakeMagnitude, cameraShakeDuration);
                StartCoroutine(shake);
            }

            switch (_playerRigidbody.velocity.y)
            {
                case < 0:
                    _fallTime += Time.deltaTime;
                    break;
                case 0 when _fallTime > 0:
                {
                    // TODO: Fall time cutoff parameter
                    if (_fallTime > 0.5f)
                        StartCoroutine(Shake(cameraShakeMagnitude * shakeFallMultiplier, cameraShakeDuration));
                
                    _fallTime = 0;
                    break;
                }
            }
            
            if (_facingRight && _moveHorizontal < 0f || !_facingRight && _moveHorizontal > 0f)
            {
                Flip();
            }
        }

        private static IEnumerator Shake(float magnitude, float duration)
        {
            var originalPosition = new Vector3(0, 0, 0);
            var elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                // TODO: Repeated
                var xOffset = Random.Range(-0.5f, 0.5f) * magnitude;
                var yOffset = Random.Range(-0.5f, 0.5f) * magnitude;

                Camera.main.transform.localPosition = new Vector3(xOffset, yOffset, originalPosition.z);

                elapsedTime += Time.deltaTime;
                
                yield return null;
            }

            Camera.main.transform.localPosition = originalPosition;
        }

        private void Flip()
        {
            var o = gameObject;
            var currentScale = o.transform.localScale;
            currentScale.x *= -1;

            // Temporarily detach pivot point which follow the mouse during the flip 
            foreach (var excluded in excludeFromFlip)
                excluded.parent = null;

            o.transform.localScale = currentScale;

            foreach (var excluded in excludeFromFlip)
                excluded.parent = o.transform;

            _facingRight = !_facingRight;
        }
    }
}