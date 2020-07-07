using System;
using UnityEngine;
using Zenject;

namespace Cannon
{
    [RequireComponent(typeof(LineRenderer))]
    internal sealed class CannonTrajectory : MonoBehaviour
    {
        [SerializeField, HideInInspector] private LineRenderer lineRenderer;
        private readonly Vector3 gravity = new Vector3(0f, -30f, 0f);
        private CannonBarrel.Settings barrelSettings;

        private float fireForceMultiplier;
        private Vector3[] newPositions;
        private new Transform transform;

        private void Reset()
        {
            lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.positionCount = 100;
            newPositions = new Vector3[lineRenderer.positionCount];
            lineRenderer.SetPositions(newPositions);
            lineRenderer.widthMultiplier = 1.5f;
            lineRenderer.startWidth = 0.5f;
            lineRenderer.endWidth = 40f;
            lineRenderer.startColor = lineRenderer.endColor = new Color(0f, 1f, 0f, 0.588f);
            lineRenderer.widthCurve = AnimationCurve.EaseInOut(0f, 1f/3f, 1f, 1f);
        }

        private void Start()
        {
            transform = base.transform;
            fireForceMultiplier = 1 / Time.fixedDeltaTime;
        }

        private void LateUpdate()
        {
            DrawTrajectory();
        }

        private void OnValidate()
        {
            newPositions = new Vector3[lineRenderer.positionCount];
        }

        private void DrawTrajectory()
        {
            var accumulatedPosition = transform.position;
            var velocity = transform.forward * (barrelSettings.Force);

            newPositions[0] = accumulatedPosition;
            for (var i = 1; i < newPositions.Length; i++)
            {
                accumulatedPosition += velocity * Time.fixedDeltaTime;
                velocity += gravity * Time.fixedDeltaTime;
                newPositions[i] = accumulatedPosition;
            }

            lineRenderer.SetPositions(newPositions);
        }

        [Inject]
        private void Construct(CannonBarrel.Settings barrelSettings)
        {
            this.barrelSettings = barrelSettings;
        }
    }
}