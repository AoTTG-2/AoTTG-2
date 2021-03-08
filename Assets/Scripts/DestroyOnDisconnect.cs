using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;

public class DestroyOnDisconnect : PunBehaviour
{
    public override void OnDisconnectedFromPhoton()
    {
        Destroy(this.gameObject);
    }
}
