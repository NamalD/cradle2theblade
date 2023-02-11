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
            }

            if (_facingRight && _moveHorizontal < 0f || !_facingRight && _moveHorizontal > 0f)
            {
                Flip();
            }
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