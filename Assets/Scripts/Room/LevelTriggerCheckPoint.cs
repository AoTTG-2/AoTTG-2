using Assets.Scripts.Characters.Humans;
using Assets.Scripts.Gamemode;
using System;
using UnityEngine;
using Assets.Scripts.Services;

namespace Assets.Scripts.Room
{
    [Obsolete("Use RacingCheckpointTrigger instead")]
    public class LevelTriggerCheckPoint : MonoBehaviour
    {
        HumanSpawner spawner;
        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                if (other.gameObject.GetComponent<Hero>().photonView.isMine)
                {
                    Service.Spawn.RespawnSpawner = spawner;
                }
            }
        }

        private void Start()
        {
            gameObject.AddComponent<HumanSpawner>();
            spawner = GetComponent<HumanSpawner>();
            Service.Spawn.Remove(spawner);
        }
    }
}
