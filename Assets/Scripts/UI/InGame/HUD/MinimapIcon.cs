using UnityEngine;

namespace Assets.Scripts.UI.InGame.HUD
{
    public class MinimapIcon : MonoBehaviour
    {
        private Rigidbody parentRigidbody;
        private float targetY = 0f;
        private float currentY = 0f;
        private readonly float speed = 1000f;

        private void Awake()
        {
            parentRigidbody = GetComponentInParent<Rigidbody>();
        }

        private void LateUpdate()
        {
            transform.position = new Vector3(transform.parent.position.x, 605f, transform.parent.position.z);

            if (parentRigidbody != null)
            {
                if (parentRigidbody.velocity.magnitude <= 1f)
                    targetY = transform.parent.rotation.eulerAngles.y;
                else
                    targetY = Quaternion.LookRotation(parentRigidbody.velocity).eulerAngles.y;
            }

            currentY = Mathf.MoveTowardsAngle(currentY, targetY, speed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(new Vector3(90f, currentY, 0f));
        }
    }
}