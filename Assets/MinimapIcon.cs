using UnityEngine;

namespace AOT.UI
{
    public class MinimapIcon : MonoBehaviour
    {
        private Rigidbody rb;
        private float targetY = 0f;
        private float currentY = 0f;
        private readonly float speed = 1000f;

        private void Start()
        {
            rb = GetComponentInParent<Rigidbody>();
        }

        private void LateUpdate()
        {
            if (transform != null)
                transform.position = new Vector3(transform.parent.position.x, 245f, transform.parent.position.z);

            if (rb != null)
            {
                if (rb.velocity.magnitude <= 1f)
                    targetY = transform.parent.rotation.eulerAngles.y;
                else
                    targetY = Quaternion.LookRotation(rb.velocity).eulerAngles.y;
            }

            currentY = Mathf.MoveTowardsAngle(currentY, targetY, speed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(new Vector3(90f, currentY, 0f));
        }
    }
}