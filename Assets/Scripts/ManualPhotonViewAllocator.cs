using System;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class ManualPhotonViewAllocator : MonoBehaviour
{
    public GameObject Prefab;

    public void AllocateManualPhotonView()
    {
        PhotonView photonView = base.gameObject.GetPhotonView();
        if (photonView == null)
        {
            Debug.LogError("Can't do manual instantiation without PhotonView component.");
        }
        else
        {
            int num = PhotonNetwork.AllocateViewID();
            object[] parameters = new object[] { num };
            photonView.RPC("InstantiateRpc", PhotonTargets.AllBuffered, parameters);
        }
    }

    [RPC]
    public void InstantiateRpc(int viewID)
    {
        GameObject go = UnityEngine.Object.Instantiate(this.Prefab, InputToEvent.inputHitPos + new Vector3(0f, 5f, 0f), Quaternion.identity) as GameObject;
        go.GetPhotonView().viewID = viewID;
        go.GetComponent<OnClickDestroy>().DestroyByRpc = true;
    }
}

