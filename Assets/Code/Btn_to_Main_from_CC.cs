using System;
using UnityEngine;

public class Btn_to_Main_from_CC : MonoBehaviour
{
    private void OnClick()
    {
        PhotonNetwork.Disconnect();
        Screen.lockCursor = false;
        Cursor.visible = true;
        IN_GAME_MAIN_CAMERA.gametype = GAMETYPE.STOP;
        GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().gameStart = false;
        GameObject.Find("InputManagerController").GetComponent<FengCustomInputs>().menuOn = false;
        UnityEngine.Object.Destroy(GameObject.Find("MultiplayerManager"));
        Application.LoadLevel("menu");
    }
}

