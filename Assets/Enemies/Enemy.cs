using Common;
using UnityEngine;

namespace Enemies
{
    [RequireComponent(typeof(EnemyAnimations))]
    [RequireComponent(typeof(Health))]
    public class Enemy : MonoBehaviour
    {
        private int _scatterLayer;
        private int _floorLayer;
        
        private EnemyAnimations _animations;
        private bool _falling;
        private float _fallTime;

        [Header("Scatter Damage")]
        [SerializeField]
        private int baseScatterDamage = 10;

        [SerializeField]
        private float highScatterThreshold = 3f;

        [Header("Fall Damage")]
        [SerializeField]
        private float fallDamageBase = 10;

        [SerializeField]
        private float fallTimeThreshold = 0.25f;

        public Health Health { get; private set; }

        private void Awake()
        {
            InitialiseComponents();
            InitialiseLayers();
        }

        private void InitialiseComponents()
        {
            _animations = GetComponent<EnemyAnimations>();

            Health = GetComponent<Health>();
            Health.onDeath.AddListener(Die);
        }

        private void InitialiseLayers()
        {
            _scatterLayer = LayerMask.NameToLayer("Scatter");
            _floorLayer = LayerMask.NameToLayer("Floor");
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

        private void FixedUpdate()
        {
            if (_falling)
            {
                _fallTime += Time.deltaTime;
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.layer == _scatterLayer)
            {
                TakeScatterCollisionDamage(collision);
            }
            else if (collision.gameObject.layer == _floorLayer)
            {
                TakeFallDamage();
            }
        }

        private void TakeScatterCollisionDamage(Collision2D collision)
        {
            var scatterDamageScale = collision.relativeVelocity.magnitude / highScatterThreshold;
            var scatterDamage = (int)(baseScatterDamage * scatterDamageScale);
            HandleDamage(scatterDamage);
        }

        private void TakeFallDamage()
        {
            if (_fallTime >= fallTimeThreshold)
            {
                var fallDamage = (int)(fallDamageBase * _fallTime * (1 / fallTimeThreshold));
                HandleDamage(fallDamage);
            }

            _fallTime = 0;
            _falling = false;
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.layer != _floorLayer)
                return;

            _falling = true;
        }
    }
}