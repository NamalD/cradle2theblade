using UnityEngine;

namespace Enemies
{
    [RequireComponent(typeof(EnemyAnimations))]
    public class Enemy : MonoBehaviour
    {
        private EnemyAnimations _animations;
    
        [SerializeField]
        private int maxHealth = 100;

        [SerializeField]
        private int scatterDamage = 10;

        public int MaxHealth => maxHealth;

        public int CurrentHealth { get; private set; }

        public void Start()
        {
            _animations = GetComponent<EnemyAnimations>();
            CurrentHealth = maxHealth;
        }

        public void Attack(int damage)
        {
            HandleDamage(damage);
        }

        private void HandleDamage(int damage)
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
            Destroy(gameObject);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.layer != LayerMask.NameToLayer("Scatter"))
                return;

            // TODO: Scale damage based on speed of collision
            HandleDamage(scatterDamage);
        }
    }
}
