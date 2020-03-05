using System;
using UnityEngine;

public class BodyPushBox : MonoBehaviour
{
    public GameObject parent;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "bodyCollider")
        {
            BodyPushBox component = other.gameObject.GetComponent<BodyPushBox>();
            if ((component != null) && (component.parent != null))
            {
                float num3;
                Vector3 vector = component.parent.transform.position - this.parent.transform.position;
                float radius = base.gameObject.GetComponent<CapsuleCollider>().radius;
                float num2 = base.gameObject.GetComponent<CapsuleCollider>().radius;
                vector.y = 0f;
                if (vector.magnitude > 0f)
                {
                    num3 = (radius + num2) - vector.magnitude;
                    vector.Normalize();
                }
                else
                {
                    num3 = radius + num2;
                    vector.x = 1f;
                }
                if (num3 >= 0.1f)
                {
                }
            }
        }
    }
}

