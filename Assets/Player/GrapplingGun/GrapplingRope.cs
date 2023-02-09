using System;
using UnityEngine;

namespace Player.GrapplingGun
{
    public class GrapplingRope : MonoBehaviour
    {
        [Header("General References:")]
        public GrapplingGun grapplingGun;

        public LineRenderer lineRenderer;

        [Header("General Settings:")]
        [SerializeField]
        private int precision = 40;

        [Range(0, 20)]
        [SerializeField]
        private float straightenLineSpeed = 5;

        [Header("Rope Animation Settings:")]
        public AnimationCurve ropeAnimationCurve;

        [Range(0.01f, 4)]
        [SerializeField]
        private float startWaveSize = 2;

        private float _waveSize;

        [Header("Rope Progression:")]
        public AnimationCurve ropeProgressionCurve;

        [SerializeField]
        [Range(1, 50)]
        private float ropeProgressionSpeed = 1;

        private float _moveTime;

        [HideInInspector]
        public bool isGrappling = true;

        private bool _straightLine = true;

        private void OnEnable()
        {
            _moveTime = 0;
            lineRenderer.positionCount = precision;
            _waveSize = startWaveSize;
            _straightLine = false;

            LinePointsToFirePoint();

            lineRenderer.enabled = true;
        }

        private void OnDisable()
        {
            lineRenderer.enabled = false;
            isGrappling = false;
        }

        private void LinePointsToFirePoint()
        {
            for (var i = 0; i < precision; i++)
            {
                lineRenderer.SetPosition(i, grapplingGun.firePoint.position);
            }
        }

        private void Update()
        {
            _moveTime += Time.deltaTime;
            DrawRope();
        }

        private void DrawRope()
        {
            if (!_straightLine)
            {
                if (Math.Abs(lineRenderer.GetPosition(precision - 1).x - grapplingGun.grapplePoint.x) < float.Epsilon)
                {
                    _straightLine = true;
                }
                else
                {
                    DrawRopeWaves();
                }
            }
            else
            {
                if (!isGrappling)
                {
                    grapplingGun.Grapple();
                    isGrappling = true;
                }

                if (_waveSize > 0)
                {
                    _waveSize -= Time.deltaTime * straightenLineSpeed;
                    DrawRopeWaves();
                }
                else
                {
                    _waveSize = 0;

                    if (lineRenderer.positionCount != 2)
                    {
                        lineRenderer.positionCount = 2;
                    }

                    DrawRopeNoWaves();
                }
            }
        }

        private void DrawRopeWaves()
        {
            for (var i = 0; i < precision; i++)
            {
                var delta = i / (precision - 1f);
                var offset = Vector2.Perpendicular(grapplingGun.grappleDistanceVector).normalized *
                             (ropeAnimationCurve.Evaluate(delta) * _waveSize);
                var position = grapplingGun.firePoint.position;
                var targetPosition =
                    Vector2.Lerp(position, grapplingGun.grapplePoint, delta) + offset;
                var currentPosition = Vector2.Lerp(position, targetPosition,
                    ropeProgressionCurve.Evaluate(_moveTime) * ropeProgressionSpeed);

                lineRenderer.SetPosition(i, currentPosition);
            }
        }

        private void DrawRopeNoWaves()
        {
            lineRenderer.SetPosition(0, grapplingGun.firePoint.position);
            lineRenderer.SetPosition(1, grapplingGun.grapplePoint);
        }
    }
}