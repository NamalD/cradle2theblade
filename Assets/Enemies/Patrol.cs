using System;
using Common;
using UnityEngine;

namespace Enemies
{
    public class Patrol : MonoBehaviour
    {
        private Vector2? _leftPatrolPoint;
        private Vector2? _rightPatrolPoint;
        private MovementDirection? _patrolDirection;

        private Rigidbody2D _rigidbody;

        [SerializeField]
        private float floorHitDistance = 10f;

        [SerializeField]
        private float patrolPointOffset = 10f;

        [SerializeField]
        private float patrolSpeed = 2f;

        [SerializeField]
        private LayerMask floorLayers;


        private void Awake()
        {
            _rigidbody = FindObjectOfType<Rigidbody2D>();
        }

        private void Update()
        {
            // TODO: Recalculate patrol points continuously?
            (_leftPatrolPoint, _rightPatrolPoint) = FindPatrolPoints();
            if (_leftPatrolPoint is null || _rightPatrolPoint is null)
                return;
            
            _patrolDirection ??= FindNearestPatrolDirection();
            
            // TODO: Flip on switching left to right
            // TODO: Raise event when player detected
            // TODO: Jump/run away from scatter terrain
            // TODO: Switch direction on collision with scatter
            if (_patrolDirection.HasValue)
            {
                MoveToPatrolPoint();
            }
        }

        private (Vector3?, Vector3?) FindPatrolPoints()
        {
            var origin = transform.localPosition;
            var hit = Physics2D.Raycast(
                origin,
                Vector3.down,
                floorHitDistance,
                floorLayers);

            if (!hit)
                return (null, null);

            var floorTransform = hit.transform;
            var floorRect = floorTransform.GetComponent<RectTransform>();

            var corners = new Vector3[4];
            floorRect.GetWorldCorners(corners);

            // TODO: Take boundaries into account
            // TODO: Percentage based offset
            var leftPoint = corners[(int)WorldCorner.TopLeft] * 0.6f;
            var rightPoint = corners[(int)WorldCorner.TopRight] * 0.6f;
            return (leftPoint, rightPoint);
        }

        private MovementDirection? FindNearestPatrolDirection()
        {
            if (_leftPatrolPoint == null || _rightPatrolPoint == null)
                return null;

            var localPosition = (Vector2)transform.localPosition;
            var leftDistance = (_leftPatrolPoint.Value - localPosition).magnitude;
            var rightDistance = (_rightPatrolPoint.Value - localPosition).magnitude;

            return leftDistance <= rightDistance
                ? MovementDirection.Left
                : MovementDirection.Right;
        }

        private void MoveToPatrolPoint()
        {
            if (_leftPatrolPoint is null || _rightPatrolPoint is null || _patrolDirection is null)
                return;

            _rigidbody.AddForce(Vector2.one * (int)_patrolDirection * patrolSpeed * Time.deltaTime);

            // Check if we need to switch directions
            var localX = transform.localPosition.x;
            _patrolDirection = _patrolDirection switch
            {
                MovementDirection.Left when localX <= _leftPatrolPoint.Value.x => MovementDirection.Right,
                MovementDirection.Right when localX >= _rightPatrolPoint.Value.x => MovementDirection.Left,
                _ => _patrolDirection
            };
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            
            if (_leftPatrolPoint.HasValue)
                Gizmos.DrawSphere(_leftPatrolPoint.Value, 0.1f);
            
            if (_rightPatrolPoint.HasValue)
                Gizmos.DrawSphere(_rightPatrolPoint.Value, 0.1f);
        }
    }
}