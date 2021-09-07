using Assets.Scripts.Characters.Humans;
using Assets.Scripts.Gamemode;
using System;
using UnityEngine;

namespace Assets.Scripts.Room
{
    [Obsolete("Use RacingCheckpointTrigger instead")]
    public class LevelTriggerCheckPoint : MonoBehaviour
    {
        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                if (other.gameObject.GetComponent<Hero>().photonView.isMine)
                {
                    FengGameManagerMKII.instance.checkpoint = gameObject;
                }
            }
        }

        private void Start()
        {
        }
    }
}
