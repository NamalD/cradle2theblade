using Enemies;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(PlayerAnimations))]
    public class PlayerCombat : MonoBehaviour
    {
        private PlayerAnimations _animations;
        
        [SerializeField]
        private Transform attackPoint;

        [SerializeField]
        private float attackRadius = 0.5f;

        [SerializeField]
        private int attackDamage = 10;

        [SerializeField]
        private LayerMask enemyLayers;

        private void Awake()
        {
            _animations = GetComponent<PlayerAnimations>();
        }
        
        public void Attack()
        {
            _animations.SetAttacking();
        
            // ReSharper disable once Unity.PreferNonAllocApi
            var attackCollisions = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, enemyLayers);
            foreach (var collision in attackCollisions)
            {
                collision.GetComponent<Enemy>().Attack(attackDamage);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
        }
    }
}