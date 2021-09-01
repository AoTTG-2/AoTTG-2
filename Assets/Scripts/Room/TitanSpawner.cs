using Assets.Scripts.Characters.Titan;
using Assets.Scripts.Services;
using Assets.Scripts.Services.Interface;
using Assets.Scripts.Settings;
using System;
using UnityEngine;

namespace Assets.Scripts.Room
{
    /// <summary>
    /// The TitanSpawner will act as a spawnpoint for a <see cref="TitanBase"/>, based on <see cref="Type"/>.
    /// </summary>
    public class TitanSpawner : Spawner
    {
        /// <summary>
        /// After how many seconds the titan should be spawned. Does nothing when the <see cref="Type"/> is <see cref="TitanSpawnerType.None"/>
        /// </summary>
        public float Delay = 30f;
        /// <summary>
        /// When true, the spawner will keep spawning titans per <see cref="Delay"/> until the spawn limit has been reached.
        /// </summary>
        public bool Endless;
        /// <summary>
        /// If the Type == None, then no titan will automatically be spawned.
        /// </summary>
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
