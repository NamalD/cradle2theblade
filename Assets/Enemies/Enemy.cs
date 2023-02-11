using UnityEngine;

namespace Enemies
{
    [RequireComponent(typeof(EnemyAnimations))]
    public class Enemy : MonoBehaviour
    {
        private EnemyAnimations _animations;
    
        [SerializeField]
        private int maxHealth = 100;

        public int MaxHealth => maxHealth;

        public int CurrentHealth { get; private set; }

        // TODO: Fix bug - physics is disabled when this script is enabled
        // TODO: Damage enemy on physics collision
        public void Start()
        {
            _animations = GetComponent<EnemyAnimations>();
            CurrentHealth = maxHealth;
        }

        public void Attack(int damage)
        {
            CurrentHealth -= damage;
            _animations.TriggerHurt();

            if (CurrentHealth <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            _animations.TriggerDeath();
        
            GetComponent<Collider2D>().enabled = false;
            enabled = false;
        }
    }
}
