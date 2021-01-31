using UnityEngine;

namespace AOT.UI
{
    public class MinimapIcon : MonoBehaviour
    {
        private Rigidbody rb;
        private Vector3 lastVelocity;

        private void Start()
        {
            rb = GetComponentInParent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            if (transform != null)
                transform.position = new Vector3(transform.parent.position.x, 245f, transform.parent.position.z);

            if (rb != null)
            {
                if (rb.velocity.magnitude <= 1f)
                {
                    transform.rotation = Quaternion.Euler(new Vector3(90f, transform.parent.rotation.eulerAngles.y, 0f));
                }
                else
                {
                    transform.rotation = Quaternion.Euler(new Vector3(90f, Quaternion.LookRotation(rb.velocity).eulerAngles.y, 0f));
                }
            }
        }
    }
}