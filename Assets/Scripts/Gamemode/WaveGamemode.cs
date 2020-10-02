using Assets.Scripts.Characters;
using Assets.Scripts.Characters.Titan;
using Assets.Scripts.Characters.Titan.Behavior;
using Assets.Scripts.Characters.Titan.Configuration;
using Assets.Scripts.Settings;
using Assets.Scripts.Settings.Gamemodes;
using Assets.Scripts.UI.InGame.HUD;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Gamemode
{
    public class WaveGamemode : GamemodeBase
    {
        private WaveGamemodeSettings Settings => GameSettings.Gamemode as WaveGamemodeSettings;

        private int highestWave = 1;
        public int Wave = 1;

        protected override void SetStatusTop()
        {
            var content = $"Titan left: {FactionService.GetAllHostile(PlayerService.Self)} Wave : {Wave}";
            UiService.SetMessage(LabelPosition.Top, content);
        }

        protected override void SetStatusTopRight()
        {
            var content = $"Time : {TimeService.GetRoundDisplayTime()}";
            UiService.SetMessage(LabelPosition.TopRight, content);
        }

        protected override void OnFactionDefeated(Faction faction)
        {
            if (faction == FactionService.GetHumanity())
            {
                photonView.RPC(nameof(OnGameEndRpc), PhotonTargets.All, $"Survived {Wave} Waves!\nRestarting in {{0}}s", HumanScore, TitanScore);
            } else if (faction == FactionService.GetTitanity())
            {
                NextWave();
            }
        }

        private void NextWave()
        {
            Wave++;
            var level = FengGameManagerMKII.Level.Name;
            if (!(GameSettings.Respawn.Mode != RespawnMode.NEWROUND && (!level.StartsWith("Custom"))))
            {
                foreach (var player in PhotonNetwork.playerList)
                {
                    if (RCextensions.returnIntFromObject(player.CustomProperties[PhotonPlayerProperty.isTitan]) != 2)
                    {
                        FengGameManagerMKII.instance.photonView.RPC("respawnHeroInNewRound", player);
                    }
                }
            }
            if (Wave > highestWave)
            {
                highestWave = Wave;
            }
            if (!((Settings.MaxWave.Value != 0 || Wave <= Settings.MaxWave.Value) && (Settings.MaxWave.Value <= 0 || Wave <= Settings.MaxWave.Value)))
            {
                photonView.RPC(nameof(OnGameEndRpc), PhotonTargets.All, $"Survived All {Wave} Waves!\nRestarting in {{0}}s", HumanScore, TitanScore);
            }
            else
            {
                if (Wave % Settings.BossWave.Value == 0)
                {
                    for (int i = 0; i < Wave / Settings.BossWave.Value; i++)
                    {
                        SpawnService.Spawn<MindlessTitan>(GetWaveTitanConfiguration(Settings.BossType.Value));
                    }
                }
                else
                {
                    StartCoroutine(SpawnTitan(GameSettings.Titan.Start.Value + Wave * Settings.WaveIncrement.Value));
                }
            }
        }

        protected override void OnLevelWasLoaded()
        {
            if (!PhotonNetwork.isMasterClient) return;
            if (GameSettings.Gamemode.Name.Contains("Annie"))
            {
                PhotonNetwork.Instantiate("FemaleTitan", GameObject.Find("titanRespawn").transform.position, GameObject.Find("titanRespawn").transform.rotation, 0);
            }
            else
            {
                StartCoroutine(SpawnTitan(GameSettings.Titan.Start.Value));
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
        
        IEnumerator SpawnTitan(int titans)
        {
            var spawns = GameObject.FindGameObjectsWithTag("titanRespawn");
            for (int i = 0; i < titans; i++)
            {
                var randomSpawn = spawns[Random.Range(0, spawns.Length)];
                SpawnService.Spawn<MindlessTitan>(randomSpawn.transform.position, randomSpawn.transform.rotation,
                    GetWaveTitanConfiguration());
                yield return new WaitForEndOfFrame();
            }
        }
    }
}
