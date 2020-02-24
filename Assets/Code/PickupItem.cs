using Photon;
using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class PickupItem : Photon.MonoBehaviour, IPunObservable
{
    public static HashSet<PickupItem> DisabledPickupItems = new HashSet<PickupItem>();
    public UnityEngine.MonoBehaviour OnPickedUpCall;
    public bool PickupIsMine;
    public bool PickupOnTrigger;
    public float SecondsBeforeRespawn = 2f;
    public bool SentPickup;
    public double TimeOfRespawn;

    public void Drop()
    {
        if (this.PickupIsMine)
        {
            base.photonView.RPC("PunRespawn", PhotonTargets.AllViaServer, new object[0]);
        }
    }

    public void Drop(Vector3 newPosition)
    {
        if (this.PickupIsMine)
        {
            object[] parameters = new object[] { newPosition };
            base.photonView.RPC("PunRespawn", PhotonTargets.AllViaServer, parameters);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting && (this.SecondsBeforeRespawn <= 0f))
        {
            stream.SendNext(base.gameObject.transform.position);
        }
        else
        {
            Vector3 vector = (Vector3) stream.ReceiveNext();
            base.gameObject.transform.position = vector;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        PhotonView component = other.GetComponent<PhotonView>();
        if ((this.PickupOnTrigger && (component != null)) && component.isMine)
        {
            this.Pickup();
        }
    }

    internal void PickedUp(float timeUntilRespawn)
    {
        base.gameObject.SetActive(false);
        DisabledPickupItems.Add(this);
        this.TimeOfRespawn = 0.0;
        if (timeUntilRespawn > 0f)
        {
            this.TimeOfRespawn = PhotonNetwork.time + timeUntilRespawn;
            base.Invoke("PunRespawn", timeUntilRespawn);
        }
    }

    public void Pickup()
    {
        if (!this.SentPickup)
        {
            this.SentPickup = true;
            base.photonView.RPC("PunPickup", PhotonTargets.AllViaServer, new object[0]);
        }
    }

    [RPC]
    public void PunPickup(PhotonMessageInfo msgInfo)
    {
        if (msgInfo.sender.isLocal)
        {
            this.SentPickup = false;
        }
        if (!base.gameObject.GetActive())
        {
            Debug.Log(string.Concat(new object[] { "Ignored PU RPC, cause item is inactive. ", base.gameObject, " SecondsBeforeRespawn: ", this.SecondsBeforeRespawn, " TimeOfRespawn: ", this.TimeOfRespawn, " respawn in future: ", this.TimeOfRespawn > PhotonNetwork.time }));
        }
        else
        {
            this.PickupIsMine = msgInfo.sender.isLocal;
            if (this.OnPickedUpCall != null)
            {
                this.OnPickedUpCall.SendMessage("OnPickedUp", this);
            }
            if (this.SecondsBeforeRespawn <= 0f)
            {
                this.PickedUp(0f);
            }
            else
            {
                double num = PhotonNetwork.time - msgInfo.timestamp;
                double num2 = this.SecondsBeforeRespawn - num;
                if (num2 > 0.0)
                {
                    this.PickedUp((float) num2);
                }
            }
        }
    }

    [RPC]
    internal void PunRespawn()
    {
        DisabledPickupItems.Remove(this);
        this.TimeOfRespawn = 0.0;
        this.PickupIsMine = false;
        if (base.gameObject != null)
        {
            base.gameObject.SetActive(true);
        }
    }

    [RPC]
    internal void PunRespawn(Vector3 pos)
    {
        Debug.Log("PunRespawn with Position.");
        this.PunRespawn();
        base.gameObject.transform.position = pos;
    }

    public int ViewID
    {
        get
        {
            return base.photonView.viewID;
        }
    }
}

