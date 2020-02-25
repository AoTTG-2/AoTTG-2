using System;
using UnityEngine;

public class LevelTriggerCheckPoint : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
            {
                GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().checkpoint = base.gameObject;
            }
            else if (other.gameObject.GetComponent<HERO>().photonView.isMine)
            {
                GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().checkpoint = base.gameObject;
            }
        }
    }

    private void Start()
    {
    }
}

