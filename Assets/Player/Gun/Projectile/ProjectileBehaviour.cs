using UnityEngine;
using UnityEngine.Serialization;

namespace Player.Gun.Projectile
{
    public class ProjectileBehaviour : MonoBehaviour
    {
        [SerializeField]
        private float speed = 2.0f;

        [FormerlySerializedAs("pushbackForce")]
        [SerializeField]
        private int knockbackForce = 100;

        public Transform GunPivot { get; set; }

        private void Update()
        {
            transform.Translate(Time.deltaTime * speed * Vector3.right);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            // TODO: Damage enemies
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