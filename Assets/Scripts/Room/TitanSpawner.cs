using Assets.Scripts.Characters.Titan;
using Assets.Scripts.Services;
using Assets.Scripts.Services.Interface;
using Assets.Scripts.Settings;
using System;
using UnityEngine;

namespace Assets.Scripts.Room
{
    public class TitanSpawner : Spawner
    {
        public float Delay = 30f;
        public bool Endless;
        public TitanSpawnerType Type;
        private float Timer { get; set; }

        private static IEntityService EntityService => Service.Entity;

        public TitanSpawner()
        {
            Timer = Delay;
        }

        protected override void Awake()
        {
            Timer = Delay;
            if (Type != TitanSpawnerType.None)
            {
                tag = "Untagged";
            }
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
                    SpawnService.Spawn<FemaleTitan>(transform.position, transform.rotation, null);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(Type), Type, null);
            }
        }

        private void SpawnMindlessTitan(MindlessTitanType type)
        {
            if (EntityService.Count<MindlessTitan>() >= GameSettings.Titan.Limit.Value) return;
            SpawnService.Spawn<MindlessTitan>(transform.position, transform.rotation,
                FengGameManagerMKII.Gamemode.GetTitanConfiguration(type));
        }

    }
}
