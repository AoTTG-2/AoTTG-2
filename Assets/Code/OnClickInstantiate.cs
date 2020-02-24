using System;
using UnityEngine;

public class OnClickInstantiate : MonoBehaviour
{
    public int InstantiateType;
    private string[] InstantiateTypeNames = new string[] { "Mine", "Scene" };
    public GameObject Prefab;
    public bool showGui;

    private void OnClick()
    {
        if (PhotonNetwork.connectionStatesDetailed == PeerStates.Joined)
        {
            switch (this.InstantiateType)
            {
                case 0:
                    PhotonNetwork.Instantiate(this.Prefab.name, InputToEvent.inputHitPos + new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
                    break;

                case 1:
                    PhotonNetwork.InstantiateSceneObject(this.Prefab.name, InputToEvent.inputHitPos + new Vector3(0f, 5f, 0f), Quaternion.identity, 0, null);
                    break;
            }
        }
    }

    private void OnGUI()
    {
        if (this.showGui)
        {
            GUILayout.BeginArea(new Rect((float) (Screen.width - 180), 0f, 180f, 50f));
            this.InstantiateType = GUILayout.Toolbar(this.InstantiateType, this.InstantiateTypeNames, new GUILayoutOption[0]);
            GUILayout.EndArea();
        }
    }
}

