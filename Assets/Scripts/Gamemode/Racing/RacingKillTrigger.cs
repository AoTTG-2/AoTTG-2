using System;
using UnityEngine;

public class RacingKillTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GameObject gameObject = other.gameObject;
        if (gameObject.layer == 8)
        {
            gameObject = gameObject.transform.root.gameObject;
            if (((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER) && (gameObject.GetPhotonView() != null)) && gameObject.GetPhotonView().isMine)
            {
                Hero hero = gameObject.GetComponent<Hero>();
                if (hero != null)
                {
                    hero.markDie();
                    hero.photonView.RPC<int, string, PhotonMessageInfo>(
                        hero.netDie2,
                        PhotonTargets.All,
                        -1, "Server");
                }
            }
        }
    }
}

