using Photon;
using System;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class PickupItemSimple : Photon.MonoBehaviour
{
    public bool PickupOnCollide;
    public float SecondsBeforeRespawn = 2f;
    public bool SentPickup;

    public void OnTriggerEnter(Collider other)
    {
        PhotonView component = other.GetComponent<PhotonView>();
        if ((this.PickupOnCollide && (component != null)) && component.isMine)
        {
            this.Pickup();
        }
    }

    public void Pickup()
    {
        if (!this.SentPickup)
        {
            this.SentPickup = true;
            base.photonView.RPC("PunPickupSimple", PhotonTargets.AllViaServer, new object[0]);
        }
    }

    [RPC]
    public void PunPickupSimple(PhotonMessageInfo msgInfo)
    {
        if ((!this.SentPickup || !msgInfo.sender.isLocal) || !base.gameObject.GetActive())
        {
        }
        this.SentPickup = false;
        if (!base.gameObject.GetActive())
        {
            Debug.Log("Ignored PU RPC, cause item is inactive. " + base.gameObject);
        }
        else
        {
            double num = PhotonNetwork.time - msgInfo.timestamp;
            float time = this.SecondsBeforeRespawn - ((float) num);
            if (time > 0f)
            {
                base.gameObject.SetActive(false);
                base.Invoke("RespawnAfter", time);
            }
        }
    }

    public void RespawnAfter()
    {
        if (base.gameObject != null)
        {
            base.gameObject.SetActive(true);
        }
    }
}

