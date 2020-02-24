using Photon;
using System;
using UnityEngine;

public class SmoothSyncMovement : Photon.MonoBehaviour
{
    public Vector3 correctCameraPos;
    public Quaternion correctCameraRot;
    private Vector3 correctPlayerPos = Vector3.zero;
    private Quaternion correctPlayerRot = Quaternion.identity;
    private Vector3 correctPlayerVelocity = Vector3.zero;
    public bool disabled;
    public bool noVelocity;
    public bool PhotonCamera;
    public float SmoothingDelay = 5f;

    public void Awake()
    {
        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
        {
            base.enabled = false;
        }
        this.correctPlayerPos = base.transform.position;
        this.correctPlayerRot = base.transform.rotation;
        if (base.GetComponent<Rigidbody>() == null)
        {
            this.noVelocity = true;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(base.transform.position);
            stream.SendNext(base.transform.rotation);
            if (!this.noVelocity)
            {
                stream.SendNext(base.GetComponent<Rigidbody>().velocity);
            }
            if (this.PhotonCamera)
            {
                stream.SendNext(Camera.main.transform.rotation);
            }
        }
        else
        {
            this.correctPlayerPos = (Vector3) stream.ReceiveNext();
            this.correctPlayerRot = (Quaternion) stream.ReceiveNext();
            if (!this.noVelocity)
            {
                this.correctPlayerVelocity = (Vector3) stream.ReceiveNext();
            }
            if (this.PhotonCamera)
            {
                this.correctCameraRot = (Quaternion) stream.ReceiveNext();
            }
        }
    }

    public void Update()
    {
        if (!(this.disabled || base.photonView.isMine))
        {
            base.transform.position = Vector3.Lerp(base.transform.position, this.correctPlayerPos, Time.deltaTime * this.SmoothingDelay);
            base.transform.rotation = Quaternion.Lerp(base.transform.rotation, this.correctPlayerRot, Time.deltaTime * this.SmoothingDelay);
            if (!this.noVelocity)
            {
                base.GetComponent<Rigidbody>().velocity = this.correctPlayerVelocity;
            }
        }
    }
}

