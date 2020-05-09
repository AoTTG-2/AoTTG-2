using Assets.Scripts.UI.Elements;
using UnityEngine;

namespace Assets.Scripts.Gamemode
{
    public class WaveGamemode : GamemodeBase
    {

        [UiElement("Start Wave", "What is the start wave?")]
        public int StartWave { get; set; } = 1;
        [UiElement("Max Wave", "What is the current wave?")]
        public int MaxWave { get; set; } = 20;
        private int highestWave = 1;
        [UiElement("Wave Increment", "How many titans will spawn per wave?")]
        public int WaveIncrement { get; set; } = 2;
        public bool PunkWave { get; set; } = true;
        private readonly int _punkWave = 5;
        public int Wave = 1;

        public WaveGamemode()
        {
            GamemodeType = GamemodeType.Wave;
            TitanChaseDistanceEnabled = false;
            Titans = 3;
            RespawnMode = RespawnMode.NEWROUND;
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
            if (IsSinglePlayer)
            {
                return $"Survive {Wave} Waves!\n Press {FengGameManagerMKII.instance.inputManager.inputString[InputCode.restart]} to Restart.\n\n\n";
            }
            return $"Survive {Wave} Waves!\nGame Restart in {(int) gameEndCd}s\n\n";
        }

        public override string GetRoundEndedMessage()
        {
            return $"Highest Wave : {highestWave}";
        }

        public override void OnLevelWasLoaded(Level level, bool isMasterClient = false)
        {
            base.OnLevelWasLoaded(level, isMasterClient);
            if (!isMasterClient) return;
            if (Name.Contains("Annie"))
            {
                PhotonNetwork.Instantiate("FEMALE_TITAN", GameObject.Find("titanRespawn").transform.position, GameObject.Find("titanRespawn").transform.rotation, 0);
            }
            else
            {
                int num4 = 90;
                if (Difficulty == 1)
                {
                    num4 = 70;
                }
                FengGameManagerMKII.instance.spawnTitanCustom("titanRespawn", num4, Titans, false);
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
            Wave = StartWave;
            base.OnRestart();
        }

        public override void OnTitanKilled(string titanName)
        {
            if (!IsAllTitansDead()) return;
            Wave++;
            var level = FengGameManagerMKII.Level.Name;
            if (!(RespawnMode != RespawnMode.NEWROUND && (!level.StartsWith("Custom")) || (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.MULTIPLAYER)))
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
            if (!((MaxWave != 0 || Wave <= MaxWave) && (MaxWave <= 0 || Wave <= MaxWave)))
            {
                FengGameManagerMKII.instance.gameWin2();
            }
            else
            {
                int abnormal = 90;
                if (Difficulty == 1)
                {
                    abnormal = 70;
                }
                if (!IsEnabled(TitanType.TYPE_PUNK))
                {
                    FengGameManagerMKII.instance.spawnTitanCustom("titanRespawn", abnormal, Wave + 2, false);
                } 
                else if (Wave % _punkWave == 0)
                {
                    FengGameManagerMKII.instance.spawnTitanCustom("titanRespawn", abnormal, Wave / _punkWave, true);
                }
                else
                {
                    FengGameManagerMKII.instance.spawnTitanCustom("titanRespawn", abnormal, Wave + 2, false);
                }
            }
        }
    }
}
