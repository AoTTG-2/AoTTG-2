using Assets.Scripts.Characters.Titan;
using System;
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
            if (Type != TitanSpawnerType.None)
            {
                tag = "Untagged";
            }
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
                    SpawnMindlessTitan(MindlessTitanType.Normal);
                    break;
                case TitanSpawnerType.Aberrant:
                    SpawnMindlessTitan(MindlessTitanType.Abberant);
                    break;
                case TitanSpawnerType.Jumper:
                    SpawnMindlessTitan(MindlessTitanType.Jumper);
                    break;
                case TitanSpawnerType.Punk:
                    SpawnMindlessTitan(MindlessTitanType.Punk);
                    break;
                case TitanSpawnerType.Crawler:
                    SpawnMindlessTitan(MindlessTitanType.Crawler);
                    break;
                case TitanSpawnerType.Annie:
                    PhotonNetwork.Instantiate("FEMALE_TITAN", base.transform.position, base.transform.rotation, 0);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(Type), Type, null);
            }
        }

        private void SpawnMindlessTitan(MindlessTitanType type)
        {
            if (FengGameManagerMKII.instance.getTitans().Count >= FengGameManagerMKII.Gamemode.Settings.TitanLimit) return;
            FengGameManagerMKII.instance.SpawnTitan(transform.position, transform.rotation, FengGameManagerMKII.Gamemode.GetTitanConfiguration(type));
        }

    }
}
