using Photon;
using System;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class MoveByKeys : Photon.MonoBehaviour
{
    public float speed = 10f;

    private void Start()
    {
        base.enabled = base.photonView.isMine;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            Transform transform = base.transform;
            transform.position += (Vector3) (Vector3.left * (this.speed * Time.deltaTime));
        }
        if (Input.GetKey(KeyCode.D))
        {
            Transform transform2 = base.transform;
            transform2.position += (Vector3) (Vector3.right * (this.speed * Time.deltaTime));
        }
        if (Input.GetKey(KeyCode.W))
        {
            Transform transform3 = base.transform;
            transform3.position += (Vector3) (Vector3.forward * (this.speed * Time.deltaTime));
        }
        if (Input.GetKey(KeyCode.S))
        {
            Transform transform4 = base.transform;
            transform4.position += (Vector3) (Vector3.back * (this.speed * Time.deltaTime));
        }
    }
}

