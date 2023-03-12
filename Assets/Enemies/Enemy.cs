using Common;
using UnityEngine;

namespace Enemies
{
    [RequireComponent(typeof(EnemyAnimations))]
    [RequireComponent(typeof(Health))]
    public class Enemy : MonoBehaviour
    {
        private EnemyAnimations _animations;
        private bool _falling;
        private float _fallTime;
        private EnemyMode _mode = EnemyMode.Patrol;

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

        // TODO: Follow player AI
        // TODO: Grapple to player AI
        private void Awake()
        {
            InitialiseComponents();
        }

        private void InitialiseComponents()
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

        private void Update()
        {
            // TODO: Recheck mode on damage

            // TODO: In chase: move to player
            // TODO: In chase: Disable patrol behaviour 
            // TODO: In chase: stick around on timeout if player leaves layer
            // TODO: In chase: Enable patrol behaviour is player left + timed out
            // TODO: In chase: attack if near player 
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
            if (collision.gameObject.layer == Layers.Instance.Scatter)
            {
                TakeScatterCollisionDamage(collision);
            }
            else if (collision.gameObject.layer == Layers.Instance.Floor)
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
            if (collision.gameObject.layer != Layers.Instance.Floor)
                return;

            _falling = true;
        }
    }
}