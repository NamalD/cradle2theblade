using Enemies;
using UnityEngine;

namespace Player.Gun.Projectile
{
    public class ProjectileBehaviour : MonoBehaviour
    {
        [SerializeField]
        private float speed = 2.0f;

        [SerializeField]
        private int knockbackForce = 100;

        [SerializeField]
        private int bulletDamage = 10;

        public Transform GunPivot { get; set; }

        private void Update()
        {
            transform.Translate(Time.deltaTime * speed * Vector3.right);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            var enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy)
            {
                enemy.Attack(bulletDamage);
            }
            
            PushCollidedObject(collision);
            Destroy(gameObject);
        }

        private void PushCollidedObject(Collision2D collision)
        {
            if (collision.rigidbody == null)
                return;
            
            var direction = (collision.transform.position - GunPivot.position).normalized;
            collision.rigidbody.AddForce(direction * knockbackForce);
        }
    }
}