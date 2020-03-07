using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private float decay;
    private float duration;
    private bool flip;
    private float R;

    private void FixedUpdate()
    {
    }

    private void shakeUpdate()
    {
        if (this.duration > 0f)
        {
            this.duration -= Time.deltaTime;
            if (this.flip)
            {
                Transform transform = base.gameObject.transform;
                transform.position += (Vector3) (Vector3.up * this.R);
            }
            else
            {
                Transform transform2 = base.gameObject.transform;
                transform2.position -= (Vector3) (Vector3.up * this.R);
            }
            this.flip = !this.flip;
            this.R *= this.decay;
        }
    }

    private void Start()
    {
    }

    public void startShake(float R, float duration, float decay = 0.95f)
    {
        if (this.duration < duration)
        {
            this.R = R;
            this.duration = duration;
            this.decay = decay;
        }
    }

    private void Update()
    {
    }
}

