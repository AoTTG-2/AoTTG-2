using System;
using UnityEngine;

public class MovementUpdate : MonoBehaviour
{
    public bool disabled;
    private Vector3 lastPosition;
    private Quaternion lastRotation;
    private Vector3 lastVelocity;
    private Vector3 targetPosition;

    private void Start()
    {
        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
        {
            this.disabled = true;
            base.enabled = false;
        }
        else if (base.GetComponent<NetworkView>().isMine)
        {
            object[] args = new object[] { base.transform.position, base.transform.rotation, base.transform.localScale, Vector3.zero };
            base.GetComponent<NetworkView>().RPC("updateMovement", RPCMode.OthersBuffered, args);
        }
        else
        {
            this.targetPosition = base.transform.position;
        }
    }

    private void Update()
    {
        if ((!this.disabled && (Network.peerType != NetworkPeerType.Disconnected)) && (Network.peerType != NetworkPeerType.Connecting))
        {
            if (base.GetComponent<NetworkView>().isMine)
            {
                if (Vector3.Distance(base.transform.position, this.lastPosition) >= 0.5f)
                {
                    this.lastPosition = base.transform.position;
                    object[] args = new object[] { base.transform.position, base.transform.rotation, base.transform.localScale, base.GetComponent<Rigidbody>().velocity };
                    base.GetComponent<NetworkView>().RPC("updateMovement", RPCMode.Others, args);
                }
                else if (Vector3.Distance(base.transform.GetComponent<Rigidbody>().velocity, this.lastVelocity) >= 0.1f)
                {
                    this.lastVelocity = base.transform.GetComponent<Rigidbody>().velocity;
                    object[] objArray2 = new object[] { base.transform.position, base.transform.rotation, base.transform.localScale, base.GetComponent<Rigidbody>().velocity };
                    base.GetComponent<NetworkView>().RPC("updateMovement", RPCMode.Others, objArray2);
                }
                else if (Quaternion.Angle(base.transform.rotation, this.lastRotation) >= 1f)
                {
                    this.lastRotation = base.transform.rotation;
                    object[] objArray3 = new object[] { base.transform.position, base.transform.rotation, base.transform.localScale, base.GetComponent<Rigidbody>().velocity };
                    base.GetComponent<NetworkView>().RPC("updateMovement", RPCMode.Others, objArray3);
                }
            }
            else
            {
                base.transform.position = Vector3.Slerp(base.transform.position, this.targetPosition, Time.deltaTime * 2f);
            }
        }
    }

    [PunRPC]
    private void updateMovement(Vector3 newPosition, Quaternion newRotation, Vector3 newScale, Vector3 veloctiy)
    {
        this.targetPosition = newPosition;
        base.transform.rotation = newRotation;
        base.transform.localScale = newScale;
        base.GetComponent<Rigidbody>().velocity = veloctiy;
    }
}

