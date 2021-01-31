using UnityEngine;

namespace AOT.UI
{
    public class MinimapIcon : MonoBehaviour
    {
        private Rigidbody rb;

        private void Start()
        {
            rb = GetComponentInParent<Rigidbody>();
        }

        private void Update()
        {
            if (transform != null)
                transform.position = new Vector3(transform.parent.position.x, 245f, transform.parent.position.z);

            if (rb != null && rb.velocity != Vector3.zero)
            {
                var y = Quaternion.LookRotation(rb.velocity).eulerAngles.y;
                transform.rotation = Quaternion.Euler(new Vector3(90f, y, 0f));
            }
        }
    }
}