using Enemies;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(PlayerAnimations))]
    [RequireComponent(typeof(PlayerMovement))]
    public class PlayerController : MonoBehaviour
    {
        // TODO: Bundle combat together
        [SerializeField]
        private Transform attackPoint;

        [SerializeField]
        private float attackRadius = 0.5f;

        private const int AttackDamage = 10;
        public int HeavyAttackCharges { get; private set; }

        [SerializeField]
        private LayerMask enemyLayers;

        private PlayerAnimations _animations;
        private PlayerMovement _movement;

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
            // TODO: Limit attacks
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Attack();
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

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
        }
    }
}