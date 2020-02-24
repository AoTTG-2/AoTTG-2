using System;
using UnityEngine;

public class BillboardScript : MonoBehaviour
{
    public void Main()
    {
    }

    public void Update()
    {
        base.transform.LookAt(Camera.main.transform.position);
        base.transform.Rotate((Vector3) (Vector3.left * -90f));
    }
}

