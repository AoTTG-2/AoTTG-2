using System;
using UnityEngine;

public class ClickToMove : MonoBehaviour
{
    public int smooth;
    private Vector3 targetPosition;

    public void Main()
    {
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Plane plane = new Plane(Vector3.up, base.transform.position);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float enter = 0f;
            if (plane.Raycast(ray, out enter))
            {
                Vector3 point = ray.GetPoint(enter);
                this.targetPosition = ray.GetPoint(enter);
                Quaternion quaternion = Quaternion.LookRotation(point - base.transform.position);
                base.transform.rotation = quaternion;
            }
        }
        base.transform.position = Vector3.Lerp(base.transform.position, this.targetPosition, Time.deltaTime * this.smooth);
    }
}

