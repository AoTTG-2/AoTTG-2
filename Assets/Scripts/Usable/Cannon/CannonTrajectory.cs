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

        private Vector3[] newPositions;
        private new Transform transform;

        private void Reset()
        {
            lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.positionCount = 25;
            newPositions = new Vector3[lineRenderer.positionCount];
            lineRenderer.SetPositions(newPositions);
        }

        private void Start()
        {
            transform = base.transform;
            
            newPositions = new Vector3[lineRenderer.positionCount];
        }

        private void LateUpdate()
        {
            DrawTrajectory();
        }

        private void DrawTrajectory()
        {
            var accumulatedPosition = transform.position;
            var velocity = transform.forward * (barrelSettings.Force);
            var timeStep = Time.fixedDeltaTime * barrelSettings.TrajectoryLengthFactor;
            newPositions[0] = accumulatedPosition;
            for (var i = 1; i < newPositions.Length; i++)
            {
                accumulatedPosition += velocity * timeStep + gravity * (0.5f * timeStep * timeStep);
                velocity += gravity * timeStep;
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