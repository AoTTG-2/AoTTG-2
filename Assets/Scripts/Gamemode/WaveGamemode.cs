﻿using Assets.Scripts.Characters;
using Assets.Scripts.Characters.Titan;
using Assets.Scripts.Characters.Titan.Behavior;
using Assets.Scripts.Characters.Titan.Configuration;
using Assets.Scripts.Extensions;
using Assets.Scripts.Room;
using Assets.Scripts.Services;
using Assets.Scripts.Settings;
using Assets.Scripts.Settings.Gamemodes;
using Assets.Scripts.UI.InGame.HUD;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Gamemode
{
    public class WaveGamemode : GamemodeBase
    {
        public override GamemodeType GamemodeType { get; } = GamemodeType.Wave;
        private WaveGamemodeSettings Settings => GameSettings.Gamemode as WaveGamemodeSettings;

        private int HighestWave { get; set; } = 1;
        public int Wave { get; set; } = 1;

        protected override void SetStatusTop()
        {
            var content = $"{Localization.Gamemode.Shared.GetLocalizedString("ENEMY_LEFT", FactionService.CountHostile(PlayerService.Self))} | {Localization.Gamemode.Wave.GetLocalizedString("WAVE")} : {Wave}";
            UiService.SetMessage(LabelPosition.Top, content);
        }

        protected override void SetStatusTopRight()
        {
            var content = $"{Localization.Common.GetLocalizedString("TIME")} : {TimeService.GetRoundDisplayTime()}";
            UiService.SetMessage(LabelPosition.TopRight, content);
        }

        private void RoundFinish(string translationKey)
        {
            photonView.RPC(nameof(OnGameEndRpc), PhotonTargets.All,
                $"{Localization.Gamemode.Wave.GetLocalizedString(translationKey, Wave)}" +
                $"\n{Localization.Gamemode.Shared.GetLocalizedString("RESTART_COUNTDOWN")}",
                HumanScore, TitanScore);
        }

        protected override void OnFactionDefeated(Faction faction)
        {
            if (!PhotonNetwork.isMasterClient) return;
            if (faction == FactionService.GetHumanity())
            {
                RoundFinish("FAILED");
            }
        }
        private void WaveGamemode_OnTitanKilled<T1, T2>(T1 entity1, T2 entity2, int damage) where T1 : Entity where T2 : Entity
        {
            //This is effectively the same as OnFactionDefeated for Titanity, except it ensures that the player actually killed the last titan.
            //Otherwise OnFactionDefeated is triggered when Unity despawns the last titan, causing NextWave() to trigger as the level is loaded.
            if (Service.Faction.CountMembers(Service.Faction.GetTitanity()) == 0)
            {
                NextWave();
            }
        }

        private void NextWave()
        {
            Wave++;

            if (Wave > HighestWave)
                HighestWave = Wave;

            if (GameSettings.Respawn.Mode == RespawnMode.NewRound)
            {
                PhotonView photonView = Service.Photon.GetPhotonView();
                foreach (var player in PhotonNetwork.playerList)
                {
                    if (RCextensions.returnIntFromObject(player.CustomProperties[PhotonPlayerProperty.isTitan]) != 2
                        && RCextensions.returnBoolFromObject(PhotonNetwork.player.CustomProperties[PhotonPlayerProperty.dead]))
                    {
                        photonView.RPC(nameof(Service.Spawn.RespawnRpc), player);
                    }
                }
            }

            if (!((Settings.MaxWave.Value != 0 || Wave <= Settings.MaxWave.Value) && (Settings.MaxWave.Value <= 0 || Wave <= Settings.MaxWave.Value)))
            {
                RoundFinish("COMPLETE");
            }
            else if (Wave % Settings.BossWave.Value == 0)
            {
                StartCoroutine(SpawnBossTitan(Wave / Settings.BossWave.Value));
            }
            else
            {
                SpawnTitans(GameSettings.Titan.Start.Value + (Wave - 1) * Settings.WaveIncrement.Value, GetWaveTitanConfiguration);
            }
        }

        protected override void Level_OnLevelLoaded(int scene, Level level)
        {
            base.Level_OnLevelLoaded(scene, level);
            if (!PhotonNetwork.isMasterClient) return;
            if (GameSettings.Gamemode.Name.Contains("Annie"))
            {
                Transform spawnLocation = GetTitanSpawnLocation();
                PhotonNetwork.Instantiate("FemaleTitan", spawnLocation.position, spawnLocation.rotation, 0);
            }
            else
            {
                Wave = Settings.StartWave.Value;
                SpawnTitans(GameSettings.Titan.Start.Value, GetWaveTitanConfiguration);
            }
        }

        public override void OnRestart()
        {
            Wave = Settings.StartWave.Value;
            base.OnRestart();
        }

        private TitanConfiguration GetWaveTitanConfiguration()
        {
            var configuration = GetTitanConfiguration();
            configuration.Behaviors.Add(new WaveBehavior());
            configuration.ViewDistance = 999999f;
            return configuration;
        }

        private TitanConfiguration GetWaveTitanConfiguration(MindlessTitanType type)
        {
            var configuration = GetTitanConfiguration(type);
            configuration.Behaviors.Add(new WaveBehavior());
            configuration.ViewDistance = 999999f;
            return configuration;
        }

        private IEnumerator SpawnBossTitan(int titans)
        {
            for (var i = 0; i < titans; i++)
            {
                if (EntityService.Count<MindlessTitan>() >= GameSettings.Titan.Limit.Value) break;
                var randomSpawn = GetTitanSpawnLocation();
                SpawnService.Spawn<MindlessTitan>(randomSpawn.position, randomSpawn.rotation,
                    GetWaveTitanConfiguration(Settings.BossType.Value));
                yield return new WaitForEndOfFrame();
            }
        }
        protected override void Awake()
        {
            base.Awake();
            Service.Spawn.OnTitanKilled += WaveGamemode_OnTitanKilled;
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            StopAllCoroutines();
            Service.Spawn.OnTitanKilled -= WaveGamemode_OnTitanKilled;
        }
    }
}
