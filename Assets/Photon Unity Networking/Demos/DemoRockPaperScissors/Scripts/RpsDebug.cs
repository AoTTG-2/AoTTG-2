using UnityEngine;
using System.Collections;
using UnityEngine.UI;

#pragma warning disable 649

public class RpsDebug : MonoBehaviour {

    [SerializeField]
    private Button ConnectionDebugButton;

    public bool ShowConnectionDebug;


    public void ToggleConnectionDebug()
    {
        ShowConnectionDebug = !ShowConnectionDebug;
    }

    public void Update()
    {
        if (this.ShowConnectionDebug)
        { ConnectionDebugButton.GetComponentInChildren<Text>().text = PhotonNetwork.connectionStateDetailed.ToString(); }
        else
        {
            ConnectionDebugButton.GetComponentInChildren<Text>().text = "";
        }
    }
}
