using System;
using UnityEngine;

public class BTN_Enter_PWD : MonoBehaviour
{
    private void OnClick()
    {
        string text = GameObject.Find("InputEnterPWD").GetComponent<UIInput>().label.text;
        SimpleAES eaes = new SimpleAES();
        if (text == eaes.Decrypt(PanelMultiJoinPWD.Password))
        {
            PhotonNetwork.JoinRoom(PanelMultiJoinPWD.roomName);
        }
        else
        {
            NGUITools.SetActive(GameObject.Find("UIRefer").GetComponent<UIMainReferences>().PanelMultiPWD, false);
            NGUITools.SetActive(GameObject.Find("UIRefer").GetComponent<UIMainReferences>().panelMultiROOM, true);
            GameObject.Find("PanelMultiROOM").GetComponent<PanelMultiJoin>().refresh();
        }
    }
}

