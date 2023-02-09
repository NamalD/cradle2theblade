using UnityEngine;
using UnityEngine.Serialization;

namespace Player.GrapplingGun
{
    public class GrapplingGun : MonoBehaviour
    {
        [Header("Scripts Ref:")]
        public GrapplingRope grappleRope;

        [Header("Layers Settings:")]
        [SerializeField]
        private bool grappleToAll;

        [SerializeField]
        // TODO: Make this a LayerMask instead?
        private int grappleableLayerNumber = 9;

        [Header("Main Camera:")]
        public Camera mainCamera;

        [Header("Transform Ref:")]
        public Transform gunHolder;

        public Transform gunPivot;
        public Transform firePoint;

        [Header("Physics Ref:")]
        public SpringJoint2D springJoint2D;

        [FormerlySerializedAs("m_rigidbody")]
        public Rigidbody2D mainRigidbody;

        [Header("Rotation:")]
        [SerializeField]
        private bool rotateOverTime = true;

        [Range(0, 60)]
        [SerializeField]
        private float rotationSpeed = 4;

        [Header("Distance:")]
        [SerializeField]
        private bool hasMaxDistance;

        [SerializeField]
        private float maxDistance = 20;

        private enum LaunchType
        {
            TransformLaunch,
            PhysicsLaunch
        }

        [Header("Launching:")]
        [SerializeField]
        private bool launchToPoint = true;

        [SerializeField]
        private LaunchType launchType = LaunchType.PhysicsLaunch;

        [SerializeField]
        private float launchSpeed = 1;

        [Header("No Launch To Point")]
        [SerializeField]
        private bool autoConfigureDistance;

        [SerializeField]
        private float targetDistance = 3;

        [SerializeField]
        private float targetFrequency = 1;

        [HideInInspector]
        public Vector2 grapplePoint;

        [HideInInspector]
        public Vector2 grappleDistanceVector;

        private float _startGravityScale;

        private void Start()
        {
            grappleRope.enabled = false;
            springJoint2D.enabled = false;
            _startGravityScale = mainRigidbody.gravityScale;
        }

        private void Update()
        {
            // TODO: Let PlayerController handle grapple bindings
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                SetGrapplePoint();
            }
            else if (Input.GetKey(KeyCode.Mouse1))
            {
                if (grappleRope.enabled)
                {
                    // RotateGun(grapplePoint, false);
                }
                else
                {
                    Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                    // RotateGun(mousePos, true);
                }

                if (!launchToPoint || !grappleRope.isGrappling)
                {
                    // Debug.Log($"Launch: {launchToPoint}, grappling: {grappleRope.isGrappling}");
                    return;
                }

                if (launchType != LaunchType.TransformLaunch)
                    return;

                Vector2 firePointDistance = firePoint.position - gunHolder.localPosition;
                var targetPos = grapplePoint - firePointDistance;
                gunHolder.position = Vector2.Lerp(gunHolder.position, targetPos, Time.deltaTime * launchSpeed);
            }
            else if (Input.GetKeyUp(KeyCode.Mouse1))
            {
                grappleRope.enabled = false;
                springJoint2D.enabled = false;
                mainRigidbody.gravityScale = _startGravityScale;
            }
            else
            {
                Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                // RotateGun(mousePos, true);
            }
        }

        // TODO: Rotate to cursor
        // private void RotateGun(Vector3 lookPoint, bool allowRotationOverTime)
        // {
        //     var distanceVector = lookPoint - gunPivot.position;
        //
        //     var angle = Mathf.Atan2(distanceVector.y, distanceVector.x) * Mathf.Rad2Deg;
        //     if (rotateOverTime && allowRotationOverTime)
        //     {
        //         gunPivot.rotation = Quaternion.Lerp(gunPivot.rotation, Quaternion.AngleAxis(angle, Vector3.forward),
        //             Time.deltaTime * rotationSpeed);
        //     }
        //     else
        //     {
        //         gunPivot.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        //     }
        // }

        private void SetGrapplePoint()
        {
            Vector2 distanceVector = mainCamera.ScreenToWorldPoint(Input.mousePosition) - gunPivot.position;
            if (!Physics2D.Raycast(firePoint.position, distanceVector.normalized))
                return;

            var hit = Physics2D.Raycast(firePoint.position, distanceVector.normalized);
            // TODO: If grappling to all - discard boundary hits
            if (!grappleToAll && hit.transform.gameObject.layer != grappleableLayerNumber)
                return;

            if (hasMaxDistance && Vector2.Distance(hit.point, firePoint.position) > maxDistance)
                return;
            
            grapplePoint = hit.point;
            grappleDistanceVector = grapplePoint - (Vector2)gunPivot.position;
            grappleRope.enabled = true;
        }

        public void Grapple()
        {
            springJoint2D.autoConfigureDistance = false;
            if (!launchToPoint && !autoConfigureDistance)
            {
                springJoint2D.distance = targetDistance;
                springJoint2D.frequency = targetFrequency;
            }

            if (!launchToPoint)
            {
                if (autoConfigureDistance)
                {
                    springJoint2D.autoConfigureDistance = true;
                    springJoint2D.frequency = 0;
                }

                springJoint2D.connectedAnchor = grapplePoint;
                springJoint2D.enabled = true;
            }
            else
            {
                switch (launchType)
                {
                    case LaunchType.PhysicsLaunch:
                        springJoint2D.connectedAnchor = grapplePoint;

                        Vector2 distanceVector = firePoint.position - gunHolder.position;

                        springJoint2D.distance = distanceVector.magnitude;
                        springJoint2D.frequency = launchSpeed;
                        springJoint2D.enabled = true;
                        break;
                    case LaunchType.TransformLaunch:
                        mainRigidbody.gravityScale = 0;
                        mainRigidbody.velocity = Vector2.zero;
                        break;
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (firePoint != null && hasMaxDistance)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(firePoint.position, maxDistance);
            }
        }
    }
}