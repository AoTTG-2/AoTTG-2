using System;
using UnityEngine;

public class SynchronizeLights : MonoBehaviour
{
    public Light light0;
    public Light light1;

    private void LateUpdate()
    {
        if (this.light0 != null)
        {
            Vector3 vector = (Vector3) (this.light0.transform.rotation * new Vector3(0f, 0f, -1f));
            base.GetComponent<Renderer>().material.SetVector("_LightDirection0", new Vector4(vector.x, vector.y, vector.z, 0f));
            base.GetComponent<Renderer>().material.SetColor("_MyLightColor0", this.light0.color);
        }
        if (this.light1 != null)
        {
            Vector3 vector2 = (Vector3) (this.light1.transform.rotation * new Vector3(0f, 0f, -1f));
            base.GetComponent<Renderer>().material.SetVector("_LightDirection1", new Vector4(vector2.x, vector2.y, vector2.z, 0f));
            base.GetComponent<Renderer>().material.SetColor("_MyLightColor1", this.light1.color);
        }
    }
}

