using Assets.Scripts.Characters.Titan;
using Assets.Scripts.Characters.Titan.Behavior;
using Assets.Scripts.Gamemode.Settings;
using UnityEngine;

namespace Assets.Scripts.Gamemode
{
    public class WaveGamemode : GamemodeBase
    {
        private int highestWave = 1;

        public new WaveGamemodeSettings Settings { get; set; }

        public bool PunkWave { get; set; } = true;
        private readonly int _punkWave = 5;
        public int Wave = 1;

        public WaveGamemode()
        {
            Settings = new WaveGamemodeSettings
            {
                GamemodeType = GamemodeType.Wave,
                TitanChaseDistanceEnabled = false,
                Titans = 3,
                RespawnMode = RespawnMode.NEWROUND
            };
        }

        public override void SetSettings(GamemodeSettings settings)
        {
            Settings = settings as WaveGamemodeSettings;
        }

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
            if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
            {
                var content = "Time : ";
                var length = totalRoomTime;
                return content + length.ToString();
            }
            return base.GetGamemodeStatusTopRight(time, totalRoomTime);
        }

        public override string GetVictoryMessage(float timeUntilRestart, float totalServerTime = 0f)
        {
            if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
            {
                return "Survive All Waves!\n Press " + FengGameManagerMKII.instance.inputManager.inputString[InputCode.restart] + " to Restart.\n\n\n";
            }
            return $"Survive All Waves!\nGame Restart in {(int) timeUntilRestart}s\n\n";
        }

        public override string GetDefeatMessage(float gameEndCd)
        {
            if (Settings.IsSinglePlayer)
            {
                return $"Survive {Wave} Waves!\n Press {FengGameManagerMKII.instance.inputManager.inputString[InputCode.restart]} to Restart.\n\n\n";
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
            if (Settings.Name.Contains("Annie"))
            {
                PhotonNetwork.Instantiate("FEMALE_TITAN", GameObject.Find("titanRespawn").transform.position, GameObject.Find("titanRespawn").transform.rotation, 0);
            }
            else
            {
                for (int i = 0; i < Settings.Titans; i++)
                {
                    FengGameManagerMKII.instance.SpawnTitan(GetWaveTitanConfiguration());
                }
            }
        }

        public override void OnSetTitanType(ref int titanType, bool flag)
        {
            if (Wave % _punkWave != 0 && !flag)
            {
                titanType = 1;
            }
        }

        public override void OnRestart()
        {
            Wave = Settings.StartWave;
            base.OnRestart();
        }

        private TitanConfiguration GetWaveTitanConfiguration()
        {
            var configuration = GetTitanConfiguration();
            configuration.Behaviors.Add(new WaveBehavior());
            return configuration;
        }

        private TitanConfiguration GetWaveTitanConfiguration(MindlessTitanType type)
        {
            var configuration = GetTitanConfiguration(type);
            configuration.Behaviors.Add(new WaveBehavior());
            return configuration;
        }

        public override void OnTitanKilled(string titanName)
        {
            if (!IsAllTitansDead()) return;
            Wave++;
            var level = FengGameManagerMKII.Level.Name;
            if (!(Settings.RespawnMode != RespawnMode.NEWROUND && (!level.StartsWith("Custom")) || (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.MULTIPLAYER)))
            {
                foreach (var player in PhotonNetwork.playerList)
                {
                    if (RCextensions.returnIntFromObject(player.CustomProperties[PhotonPlayerProperty.isTitan]) != 2)
                    {
                        FengGameManagerMKII.instance.photonView.RPC("respawnHeroInNewRound", player);
                    }
                }
            }
            if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
            {
                //this.sendChatContentInfo("<color=#A8FF24>Wave : " + this.wave + "</color>");
            }
            if (Wave > highestWave)
            {
                highestWave = Wave;
            }
            if (PhotonNetwork.isMasterClient)
            {
                FengGameManagerMKII.instance.RequireStatus();
            }
            if (!((Settings.MaxWave != 0 || Wave <= Settings.MaxWave) && (Settings.MaxWave <= 0 || Wave <= Settings.MaxWave)))
            {
                FengGameManagerMKII.instance.gameWin2();
            }
            else
            {
                if (Wave % _punkWave == 0)
                {
                    for (int i = 0; i < Wave / _punkWave; i++)
                    {
                        FengGameManagerMKII.instance.SpawnTitan(GetWaveTitanConfiguration(MindlessTitanType.Punk));
                    }
                }
                else
                {
                    for (int i = 0; i < Wave + Settings.WaveIncrement; i++)
                    {
                        FengGameManagerMKII.instance.SpawnTitan(GetWaveTitanConfiguration());
                    }
                }
            }
        }
    }
}
