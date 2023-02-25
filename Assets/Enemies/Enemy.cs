using UnityEngine;

namespace Enemies
{
    [RequireComponent(typeof(EnemyAnimations))]
    public class Enemy : MonoBehaviour
    {
        private EnemyAnimations _animations;

        [SerializeField]
        private int maxHealth = 100;

        [Header("Scatter Damage")]
        [SerializeField]
        private int baseScatterDamage = 10;

        [SerializeField]
        private float highScatterThreshold = 3f;

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

            var scatterDamageScale = collision.relativeVelocity.magnitude / highScatterThreshold;
            var scatterDamage = (int)(baseScatterDamage * scatterDamageScale);
            Debug.Log($"Scatter damage: {scatterDamage}");
            HandleDamage(scatterDamage);
        }
    }
}