using Projectile;
using UnityEngine;

namespace Player.Gun
{
    public class GunBehaviour : MonoBehaviour
    {
        [SerializeField]
        private ProjectileBehaviour projectilePrefab;

        [SerializeField]
        private Transform gunFirePoint;

        public void Fire()
        {
            // TODO: Create a gun fire position which is independent from the grapple
            Instantiate(projectilePrefab, gunFirePoint.position, gunFirePoint.rotation);
        }
    }
}