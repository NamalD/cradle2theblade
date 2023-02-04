using Enemies;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player
{
    [RequireComponent(typeof(PlayerAnimations))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody2D playerRigidbody;

        // TODO: Bundle combat together
        [SerializeField]
        private Transform attackPoint;

        [FormerlySerializedAs("attackRange")]
        [SerializeField]
        private float attackRadius = 0.5f;

        private const int AttackDamage = 10;
        public int HeavyAttackCharges { get; private set; }

        [SerializeField]
        private LayerMask enemyLayers;

        private PlayerAnimations _animations;

        // TODO: Extract movement
        [SerializeField]
        private float moveSpeed = 3f;
    
        private float _moveHorizontal;
        private bool _facingRight = true;

        private void Awake()
        {
            _animations = GetComponent<PlayerAnimations>();
        }

        void Start()
        {
            HeavyAttackCharges = 0;
        }

        // Update is called once per frame

        void Update()
        {
            _moveHorizontal = Input.GetAxisRaw("Horizontal");
            _animations.SetMoveSpeed(_moveHorizontal);

            // TODO: Limit attacks
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Attack();
            }
        }

        private void FixedUpdate()
        {
            if (_moveHorizontal is > 0f or < 0f)
            {
                playerRigidbody.AddForce(new Vector2(_moveHorizontal * moveSpeed, 0), ForceMode2D.Impulse);
            }

            if (_facingRight && _moveHorizontal < 0f || !_facingRight && _moveHorizontal > 0f)
            {
                Flip();
            }
        }

        private void Attack()
        {
            _animations.SetAttacking();
        
            // ReSharper disable once Unity.PreferNonAllocApi
            var attackCollisions = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, enemyLayers);
            foreach (var collision in attackCollisions)
            {
                HeavyAttackCharges++;
                collision.GetComponent<Enemy>().Attack(AttackDamage);
            }
        }

        private void Flip()
        {
            var o = gameObject;
            var currentScale = o.transform.localScale;
            currentScale.x *= -1;
            o.transform.localScale = currentScale;

            _facingRight = !_facingRight;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
        }
    }
}