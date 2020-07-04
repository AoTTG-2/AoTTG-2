using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class TransformSynchronization : Photon.MonoBehaviour, IPunObservable
{
    private Vector3 correctPos = Vector3.zero;

    private Quaternion correctRot = Quaternion.identity;

    [SerializeField]
    private float smoothingDelay = 20f;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            correctPos = (Vector3) stream.ReceiveNext();
            correctRot = (Quaternion) stream.ReceiveNext();
        }
    }

    private void Awake()
    {
        if (!photonView.ObservedComponents.Contains(this))
            Debug.LogWarning($"{this} is not observed by this object's {nameof(photonView)}! {nameof(OnPhotonSerializeView)}() in this class won't be used.");
    }

#if UNITY_EDITOR

    private void Reset()
    {
        if (!photonView)
            return;

        if (photonView.ObservedComponents == null)
            photonView.ObservedComponents = new List<Component>();

        photonView.ObservedComponents.Remove(this);
        photonView.ObservedComponents.Add(this);
    }

#endif

    private void Update()
    {
        if (!photonView.isMine)
        {
            transform.position = Vector3.Lerp(transform.position, correctPos, Time.deltaTime * smoothingDelay);
            transform.rotation = Quaternion.Lerp(transform.rotation, correctRot, Time.deltaTime * smoothingDelay);
        }
    }
}