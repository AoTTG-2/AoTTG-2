using System;
using UnityEngine;

public class BTN_RESULT_TO_MAIN : MonoBehaviour
{
    private void OnClick()
    {
        Time.timeScale = 1f;
        if (PhotonNetwork.connected)
        {
            PhotonNetwork.Disconnect();
        }
        IN_GAME_MAIN_CAMERA.gametype = GAMETYPE.STOP;
        GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().gameStart = false;
        Screen.lockCursor = false;
        Cursor.visible = true;
        GameObject.Find("InputManagerController").GetComponent<FengCustomInputs>().menuOn = false;
        UnityEngine.Object.Destroy(GameObject.Find("MultiplayerManager"));
        Application.LoadLevel("menu");
    }
}

