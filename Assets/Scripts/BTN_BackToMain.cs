using System;
using UnityEngine;

public class BTN_BackToMain : MonoBehaviour
{
    private void OnClick()
    {
        NGUITools.SetActive(base.transform.parent.gameObject, false);
        NGUITools.SetActive(GameObject.Find("UIRefer").GetComponent<UIMainReferences>().panelMain, true);
        GameObject.Find("InputManagerController").GetComponent<FengCustomInputs>().menuOn = false;
        PhotonNetwork.Disconnect();
    }
}

