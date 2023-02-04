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
