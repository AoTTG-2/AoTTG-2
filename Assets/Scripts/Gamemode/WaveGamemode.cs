using Assets.Scripts.Characters.Titan;
using Assets.Scripts.Characters.Titan.Behavior;
using Assets.Scripts.Characters.Titan.Configuration;
using Assets.Scripts.Settings;
using Assets.Scripts.Settings.Gamemodes;
using Assets.Scripts.UI.Input;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Gamemode
{
    public class WaveGamemode : GamemodeBase
    {
        private WaveGamemodeSettings Settings => GameSettings.Gamemode as WaveGamemodeSettings;

        private int highestWave = 1;
        public int Wave = 1;

        public override string GetGamemodeStatusTop(int totalRoomTime = 0, int timeLeft = 0)
        {
            var content = "Titan Left: ";
            object[] objArray = new object[4];
            objArray[0] = content;
            var length = GameObject.FindGameObjectsWithTag("titan").Length;
            objArray[1] = length.ToString();
            objArray[2] = " Wave : ";
            objArray[3] = Wave;
            return string.Concat(objArray);
        }

        public override string GetGamemodeStatusTopRight(int time = 0, int totalRoomTime = 0)
        {
            if (PhotonNetwork.offlineMode)
            {
                var content = "Time : ";
                var length = totalRoomTime;
                return content + length.ToString();
            }
            return base.GetGamemodeStatusTopRight(time, totalRoomTime);
        }

        public override string GetVictoryMessage(float timeUntilRestart, float totalServerTime = 0f)
        {
            if (PhotonNetwork.offlineMode)
            {
                return $"Survive All Waves!\n Press {InputManager.GetKey(InputUi.Restart)} to Restart.\n\n\n";
            }
            return $"Survive All Waves!\nGame Restart in {(int) timeUntilRestart}s\n\n";
        }

        public override string GetDefeatMessage(float gameEndCd)
        {
            if (PhotonNetwork.offlineMode)
            {
                return $"Survive {Wave} Waves!\n Press {InputManager.GetKey(InputUi.Restart)} to Restart.\n\n\n";
            }
            return $"Survive {Wave} Waves!\nGame Restart in {(int) gameEndCd}s\n\n";
        }

        public override string GetRoundEndedMessage()
        {
            return $"Highest Wave : {highestWave}";
        }

        public override void OnLevelLoaded(Level level, bool isMasterClient = false)
        {
            base.OnLevelLoaded(level, isMasterClient);
            if (!isMasterClient) return;
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

        public override void OnTitanKilled(string titanName)
        {
            if (!IsAllTitansDead()) return;
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
            //if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
            //{
            //    //this.sendChatContentInfo("<color=#A8FF24>Wave : " + this.wave + "</color>");
            //}
            if (Wave > highestWave)
            {
                highestWave = Wave;
            }
            if (PhotonNetwork.isMasterClient)
            {
                FengGameManagerMKII.instance.RequireStatus();
            }
            if (!((Settings.MaxWave.Value != 0 || Wave <= Settings.MaxWave.Value) && (Settings.MaxWave.Value <= 0 || Wave <= Settings.MaxWave.Value)))
            {
                FengGameManagerMKII.instance.gameWin2();
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
