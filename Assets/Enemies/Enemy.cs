using Common;
using UnityEngine;

namespace Enemies
{
    [RequireComponent(typeof(EnemyAnimations))]
    [RequireComponent(typeof(Health))]
    public class Enemy : MonoBehaviour
    {
        private EnemyAnimations _animations;
        
        public Health Health { get; private set; }

        [Header("Scatter Damage")]
        [SerializeField]
        private int baseScatterDamage = 10;

        [SerializeField]
        private float highScatterThreshold = 3f;

        // TODO: Fall damage
        private void Awake()
        {
            _animations = GetComponent<EnemyAnimations>();
            
            Health = GetComponent<Health>();
            Health.onDeath.AddListener(Die);
        }

        public void Attack(int damage)
        {
            HandleDamage(damage);
        }

        private void HandleDamage(int damage)
        {
            _animations.TriggerHurt();
            Health.TakeDamage(damage);
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

            var scatterDamageScale = collision.relativeVelocity.magnitude / highScatterThreshold;
            var scatterDamage = (int)(baseScatterDamage * scatterDamageScale);
            HandleDamage(scatterDamage);
        }
    }
}