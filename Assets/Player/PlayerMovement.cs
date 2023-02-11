using System.Collections;
using UnityEngine;

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
        
        [SerializeField]
        private float moveSpeed = 3f;

        [SerializeField]
        private Transform grapplePivot;

        // TODO: Extract camera shake
        [Header("Camera Shake")]
        [SerializeField]
        private Camera mainCamera;

        [SerializeField]
        private float cameraShakeDuration = 1;
        
        [SerializeField]
        private float cameraShakeMagnitude = 1;

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
                var shake = Shake();
                StartCoroutine(shake);
            }

            if (_facingRight && _moveHorizontal < 0f || !_facingRight && _moveHorizontal > 0f)
            {
                Flip();
            }
        }

        private IEnumerator Shake()
        {
            var originalPosition = new Vector3(0, 0, 0);
            var elapsedTime = 0f;

            while (elapsedTime < cameraShakeDuration)
            {
                // TODO: Repeated
                var xOffset = Random.Range(-0.5f, 0.5f) * cameraShakeMagnitude;
                var yOffset = Random.Range(-0.5f, 0.5f) * cameraShakeMagnitude;

                mainCamera.transform.localPosition = new Vector3(xOffset, yOffset, originalPosition.z);

                elapsedTime += Time.deltaTime;
                
                yield return null;
            }

            mainCamera.transform.localPosition = originalPosition;
        }

        private void Flip()
        {
            var o = gameObject;
            var currentScale = o.transform.localScale;
            currentScale.x *= -1;

            // Temporarily detach grapple point during the flip 
            grapplePivot.parent = null;
            
            o.transform.localScale = currentScale;

            grapplePivot.parent = o.transform;
            
            _facingRight = !_facingRight;
        }
    }
}