using UnityEngine;

namespace Player.Gun.Projectile
{
    public class ProjectileBehaviour : MonoBehaviour
    {
        [SerializeField]
        private float speed = 2.0f;

        private void Update()
        {
            transform.Translate(Time.deltaTime * speed * Vector3.right);
        }

        private void OnCollisionEnter2D()
        {
            // TODO: Push enemies and boxes back
            Destroy(gameObject);
        }
    }
}
