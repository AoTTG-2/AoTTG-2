using Photon;
using System;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class OnAwakeUsePhotonView : Photon.MonoBehaviour
{
    private void Awake()
    {
        if (base.photonView.isMine)
        {
            base.photonView.RPC("OnAwakeRPC", PhotonTargets.All, new object[0]);
        }
    }

    [RPC]
    public void OnAwakeRPC()
    {
        Debug.Log("RPC: 'OnAwakeRPC' PhotonView: " + base.photonView);
    }

    [RPC]
    public void OnAwakeRPC(byte myParameter)
    {
        Debug.Log(string.Concat(new object[] { "RPC: 'OnAwakeRPC' Parameter: ", myParameter, " PhotonView: ", base.photonView }));
    }

    private void Start()
    {
        if (base.photonView.isMine)
        {
            object[] parameters = new object[] { (byte) 1 };
            base.photonView.RPC("OnAwakeRPC", PhotonTargets.All, parameters);
        }
    }
}

