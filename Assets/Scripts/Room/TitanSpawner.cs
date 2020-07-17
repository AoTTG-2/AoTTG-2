using System;
using Assets.Scripts.Characters.Titan;
using UnityEngine;
using MonoBehaviour = Photon.MonoBehaviour;

namespace Assets.Scripts.Room
{
    public class TitanSpawner : MonoBehaviour
    {
        public float Delay = 30f;
        public bool Endless;
        public TitanSpawnerType Type;
        private float Timer { get; set; }

        public TitanSpawner()
        {
            Timer = Delay;
        }

        public void Awake()
        {
            Timer = Delay;
            FengGameManagerMKII.instance.TitanSpawners.Add(this);
        }

        private void OnDestroy()
        {
            FengGameManagerMKII.instance.TitanSpawners.Remove(this);
        }

        private void Update()
        {
            if (Type == TitanSpawnerType.None) return;
            if (!PhotonNetwork.isMasterClient) return;
            Timer -= Time.deltaTime;
            if (Timer > 0f) return;

            SpawnTitan();

            if (Endless)
            {
                Timer = Delay;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void SpawnTitan()
        {
            switch (Type)
            {
                case TitanSpawnerType.None:
                    break;
                case TitanSpawnerType.Normal:
                    FengGameManagerMKII.Gamemode.SpawnTitan(MindlessTitanType.Normal);
                    break;
                case TitanSpawnerType.Aberrant:
                    break;
                case TitanSpawnerType.Jumper:
                    break;
                case TitanSpawnerType.Punk:
                    break;
                case TitanSpawnerType.Crawler:
                    break;
                case TitanSpawnerType.Annie:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(Type), Type, null);
            }
        }
    }
}
