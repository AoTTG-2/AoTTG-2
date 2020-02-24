using Photon;
using System;
using UnityEngine;

public class SelfDestroy : Photon.MonoBehaviour
{
    public float CountDown = 5f;

    private void Start()
    {
    }

    private void Update()
    {
        this.CountDown -= Time.deltaTime;
        if (this.CountDown <= 0f)
        {
            if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
            {
                UnityEngine.Object.Destroy(base.gameObject);
            }
            else if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
            {
                if (base.photonView != null)
                {
                    if (base.photonView.viewID == 0)
                    {
                        UnityEngine.Object.Destroy(base.gameObject);
                    }
                    else if (base.photonView.isMine)
                    {
                        PhotonNetwork.Destroy(base.gameObject);
                    }
                }
                else
                {
                    UnityEngine.Object.Destroy(base.gameObject);
                }
            }
        }
    }
}

