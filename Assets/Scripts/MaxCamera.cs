using System;
using UnityEngine;

[AddComponentMenu("Camera-Control/3dsMax Camera Style")]
public class MaxCamera : MonoBehaviour
{
    private float currentDistance;
    private Quaternion currentRotation;
    private float desiredDistance;
    private Quaternion desiredRotation;
    public float distance = 5f;
    public float maxDistance = 20f;
    public float minDistance = 0.6f;
    public float panSpeed = 0.3f;
    private Vector3 position;
    private Quaternion rotation;
    public Transform target;
    public Vector3 targetOffset;
    private float xDeg;
    public float xSpeed = 200f;
    private float yDeg;
    public int yMaxLimit = 80;
    public int yMinLimit = -80;
    public float ySpeed = 200f;
    public float zoomDampening = 5f;
    public int zoomRate = 40;

    private static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360f)
        {
            angle += 360f;
        }
        if (angle > 360f)
        {
            angle -= 360f;
        }
        return Mathf.Clamp(angle, min, max);
    }

    public void Init()
    {
        if (this.target == null)
        {
            GameObject obj2 = new GameObject("Cam Target") {
                transform = { position = base.transform.position + (base.transform.forward * this.distance) }
            };
            this.target = obj2.transform;
        }
        this.distance = Vector3.Distance(base.transform.position, this.target.position);
        this.currentDistance = this.distance;
        this.desiredDistance = this.distance;
        this.position = base.transform.position;
        this.rotation = base.transform.rotation;
        this.currentRotation = base.transform.rotation;
        this.desiredRotation = base.transform.rotation;
        this.xDeg = Vector3.Angle(Vector3.right, base.transform.right);
        this.yDeg = Vector3.Angle(Vector3.up, base.transform.up);
    }

    private void LateUpdate()
    {
        if ((Input.GetMouseButton(2) && Input.GetKey(KeyCode.LeftAlt)) && Input.GetKey(KeyCode.LeftControl))
        {
            this.desiredDistance -= (((Input.GetAxis("Mouse Y") * Time.deltaTime) * this.zoomRate) * 0.125f) * Mathf.Abs(this.desiredDistance);
        }
        else if (Input.GetMouseButton(0) && Input.GetKey(KeyCode.LeftAlt))
        {
            this.xDeg += (Input.GetAxis("Mouse X") * this.xSpeed) * 0.02f;
            this.yDeg -= (Input.GetAxis("Mouse Y") * this.ySpeed) * 0.02f;
            this.yDeg = ClampAngle(this.yDeg, (float) this.yMinLimit, (float) this.yMaxLimit);
            this.desiredRotation = Quaternion.Euler(this.yDeg, this.xDeg, 0f);
            this.currentRotation = base.transform.rotation;
            this.rotation = Quaternion.Lerp(this.currentRotation, this.desiredRotation, Time.deltaTime * this.zoomDampening);
            base.transform.rotation = this.rotation;
        }
        this.desiredDistance -= ((Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime) * this.zoomRate) * Mathf.Abs(this.desiredDistance);
        this.desiredDistance = Mathf.Clamp(this.desiredDistance, this.minDistance, this.maxDistance);
        this.currentDistance = Mathf.Lerp(this.currentDistance, this.desiredDistance, Time.deltaTime * this.zoomDampening);
        this.position = this.target.position - (((Vector3) ((this.rotation * Vector3.forward) * this.currentDistance)) + this.targetOffset);
        base.transform.position = this.position;
    }

    private void OnEnable()
    {
        this.Init();
    }

    private void Start()
    {
        this.Init();
    }
}

