using Player.Gun.Projectile;
using UnityEngine;

namespace Player.Gun
{
    public class GunBehaviour : MonoBehaviour
    {
        [SerializeField]
        private ProjectileBehaviour projectilePrefab;

        [SerializeField]
        private Transform gunPivot;
        
        [SerializeField]
        private Transform gunFirePoint;

        private void Update()
        {
            var mousePos = Camera.main!.ScreenToWorldPoint(Input.mousePosition);
            RotateGun(mousePos);
        }

        public void Fire()
        {
            Instantiate(projectilePrefab, gunFirePoint.position, gunFirePoint.rotation);
        }
        
        private void RotateGun(Vector3 lookPoint)
        {
            var distanceVector = lookPoint - gunPivot.position;
        
            var angle = Mathf.Atan2(distanceVector.y, distanceVector.x) * Mathf.Rad2Deg;
            gunPivot.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
}