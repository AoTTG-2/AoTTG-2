using System;
using UnityEngine;

public class MovementUpdate1 : MonoBehaviour
{
    public bool disabled;
    public Vector3 lastPosition;
    public Quaternion lastRotation;
    public Vector3 lastVelocity;

    private void Start()
    {
        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
        {
            this.disabled = true;
            base.enabled = false;
        }
        else if (base.GetComponent<NetworkView>().isMine)
        {
            object[] args = new object[] { base.transform.position, base.transform.rotation, base.transform.lossyScale };
            base.GetComponent<NetworkView>().RPC("updateMovement1", RPCMode.OthersBuffered, args);
        }
        else
        {
            base.enabled = false;
        }
    }

    private void Update()
    {
        if (!this.disabled)
        {
            object[] args = new object[] { base.transform.position, base.transform.rotation, base.transform.lossyScale };
            base.GetComponent<NetworkView>().RPC("updateMovement1", RPCMode.Others, args);
        }
    }

    [PunRPC]
    private void updateMovement1(Vector3 newPosition, Quaternion newRotation, Vector3 newScale)
    {
        base.transform.position = newPosition;
        base.transform.rotation = newRotation;
        base.transform.localScale = newScale;
    }
}

