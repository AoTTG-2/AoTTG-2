using System;
using UnityEngine;

public class SpectatorMovement : MonoBehaviour
{
    public bool disable;
    public FengCustomInputs inputManager;
    private float speed = 100f;

    private void Start()
    {
        this.inputManager = GameObject.Find("InputManagerController").GetComponent<FengCustomInputs>();
    }

    private void Update()
    {
        if (!this.disable)
        {
            float num2;
            float num3;
            float speed = this.speed;
            if (this.inputManager.isInput[InputCode.jump])
            {
                speed *= 3f;
            }
            if (this.inputManager.isInput[InputCode.up])
            {
                num2 = 1f;
            }
            else if (this.inputManager.isInput[InputCode.down])
            {
                num2 = -1f;
            }
            else
            {
                num2 = 0f;
            }
            if (this.inputManager.isInput[InputCode.left])
            {
                num3 = -1f;
            }
            else if (this.inputManager.isInput[InputCode.right])
            {
                num3 = 1f;
            }
            else
            {
                num3 = 0f;
            }
            Transform transform = base.transform;
            if (num2 > 0f)
            {
                transform.position += (Vector3) ((base.transform.forward * speed) * Time.deltaTime);
            }
            else if (num2 < 0f)
            {
                transform.position -= (Vector3) ((base.transform.forward * speed) * Time.deltaTime);
            }
            if (num3 > 0f)
            {
                transform.position += (Vector3) ((base.transform.right * speed) * Time.deltaTime);
            }
            else if (num3 < 0f)
            {
                transform.position -= (Vector3) ((base.transform.right * speed) * Time.deltaTime);
            }
            if (this.inputManager.isInput[InputCode.leftRope])
            {
                transform.position -= (Vector3) ((base.transform.up * speed) * Time.deltaTime);
            }
            else if (this.inputManager.isInput[InputCode.rightRope])
            {
                transform.position += (Vector3) ((base.transform.up * speed) * Time.deltaTime);
            }
        }
    }
}

