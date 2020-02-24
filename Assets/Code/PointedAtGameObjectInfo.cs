using System;
using UnityEngine;

[RequireComponent(typeof(InputToEvent))]
public class PointedAtGameObjectInfo : MonoBehaviour
{
    private void OnGUI()
    {
        if (InputToEvent.goPointedAt != null)
        {
            PhotonView photonView = InputToEvent.goPointedAt.GetPhotonView();
            if (photonView != null)
            {
                object[] args = new object[] { photonView.viewID, photonView.instantiationId, photonView.prefix, !photonView.isSceneView ? (!photonView.isMine ? ("owner: " + photonView.ownerId) : "mine") : "scene" };
                GUI.Label(new Rect(Input.mousePosition.x + 5f, (Screen.height - Input.mousePosition.y) - 15f, 300f, 30f), string.Format("ViewID {0} InstID {1} Lvl {2} {3}", args));
            }
        }
    }
}

