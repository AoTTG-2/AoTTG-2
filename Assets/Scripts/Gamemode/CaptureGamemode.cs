using Assets.Scripts.UI.Elements;
using UnityEngine;

namespace Assets.Scripts.Gamemode
{
    public class CaptureGamemode : GamemodeBase
    {
        public CaptureGamemode()
        {
            GamemodeType = GamemodeType.Capture;
            RespawnTime = 20f;
            PlayerTitanShifters = false;
            Titans = 0;
            TitanLimit = 25;
            TitanChaseDistance = 120f;
            SpawnTitansOnFemaleTitanDefeat = false;
            FemaleTitanDespawnTimer = 20f;
            FemaleTitanHealthModifier = 0.8f;
        }

        [UiElement("Human Point Limit", "Once this reaches 0, the titans win")]
        public int PvpTitanScoreLimit { get; set; } = 200;
        [UiElement("Titan Point Limit", "Once this reaches 0, the humans win")]
        public int PvpHumanScoreLimit { get; set; } = 200;


        [UiElement("Supply Station on Capture", "Should Supply stations spawn when a point is captured by humans?")]
        public bool SpawnSupplyStationOnHumanCapture { get; set; }

        public int PvpTitanScore;
        public int PvpHumanScore;

        private const string HumanStart = "CheckpointStartHuman";
        private const string TitanStart = "CheckpointStartTitan";

        public override string GetGamemodeStatusTop(int time = 0, int totalRoomTime = 0)
        {
            string str2 = "| ";
            for (int i = 0; i < PVPcheckPoint.chkPts.Count; i++)
            {
                str2 = str2 + (PVPcheckPoint.chkPts[i] as PVPcheckPoint).getStateString() + " ";
            }
            str2 = str2 + "|";
            var length = totalRoomTime - time;
            return $"{PvpTitanScoreLimit - PvpTitanScore} {str2} {PvpHumanScoreLimit - PvpHumanScore} \nTime : {length}";
        }

        public override void OnTitanKilled(string titanName)
        {
            if (titanName != string.Empty)
            {
                switch (titanName)
                {
                    case "Titan":
                        PvpHumanScore++;
                        break;
                    case "Aberrant":
                        PvpHumanScore += 2;
                        break;
                    case "Jumper":
                        PvpHumanScore += 3;
                        break;
                    case "Crawler":
                        PvpHumanScore += 4;
                        break;
                    case "Female Titan":
                        PvpHumanScore += 10;
                        break;
                    default:
                        PvpHumanScore += 3;
                        break;
                }
            }
            CheckWinConditions();
        }

        [PunRPC]
        public void RefreshCaptureScore(int humanScore, int titanScore, PhotonMessageInfo info)
        {
            if (!info.sender.IsMasterClient) return;
            PvpHumanScore = humanScore;
            PvpTitanScore = titanScore;
        }

        private void CheckWinConditions()
        {
            if (PhotonNetwork.isMasterClient)
            {
                FengGameManagerMKII.instance.photonView.RPC("RefreshCaptureScore", PhotonTargets.Others, HumanScore, TitanScore);
            }

            if (PvpTitanScore >= PvpTitanScoreLimit)
            {
                PvpTitanScore = PvpTitanScoreLimit;
                FengGameManagerMKII.instance.gameLose2();
            }
            else if (PvpHumanScore >= PvpHumanScoreLimit)
            {
                PvpHumanScore = PvpHumanScoreLimit;
                FengGameManagerMKII.instance.gameWin2();
            }
        }

        public void AddHumanScore(int score)
        {
            PvpHumanScore += score;
            CheckWinConditions();
        }

        public void AddTitanScore(int score)
        {
            PvpTitanScore += score;
            CheckWinConditions();
        }

        public override void OnLevelWasLoaded(Level level, bool isMasterClient = false)
        {
            base.OnLevelWasLoaded(level, isMasterClient);
            if (!FengGameManagerMKII.instance.needChooseSide && (int) FengGameManagerMKII.settings[0xf5] == 0)
            {
                if (RCextensions.returnIntFromObject(PhotonNetwork.player.CustomProperties[PhotonPlayerProperty.isTitan]) == 2)
                {
                    FengGameManagerMKII.instance.checkpoint = GameObject.Find(TitanStart);
                }
                else
                {
                    FengGameManagerMKII.instance.checkpoint = GameObject.Find(HumanStart);
                }
            }

            if (isMasterClient && FengGameManagerMKII.Level.SceneName == "OutSide")
            {
                GameObject[] objArray3 = GameObject.FindGameObjectsWithTag("titanRespawn");
                if (objArray3.Length <= 0)
                {
                    return;
                }
                for (int i = 0; i < objArray3.Length; i++)
                {
                    spawnTitanRaw(objArray3[i].transform.position, objArray3[i].transform.rotation).GetComponent<TITAN>().setAbnormalType2(TitanType.TYPE_CRAWLER, true);
                }
            }
        }

        public override GameObject SpawnNonAiTitan(Vector3 position, GameObject randomTitanRespawn)
        {
            return PhotonNetwork.Instantiate("TITAN_VER3.1", FengGameManagerMKII.instance.checkpoint.transform.position + new Vector3(Random.Range(-20, 20), 2f, Random.Range(-20, 20)), FengGameManagerMKII.instance.checkpoint.transform.rotation, 0);
        }

        public override GameObject GetPlayerSpawnLocation(string tag = "playerRespawn")
        {
            if (FengGameManagerMKII.instance.checkpoint == null)
            {
                FengGameManagerMKII.instance.checkpoint = GameObject.Find("CheckpointStartHuman");
            }
            return FengGameManagerMKII.instance.checkpoint;
        }

        public override void OnPlayerSpawned(GameObject player)
        {
            var transform = player.transform;
            transform.position += new Vector3(Random.Range(-20, 20), 2f, Random.Range(-20, 20));
        }

        public override void OnGameWon()
        {
            base.OnGameWon();
            ResetScore();
        }

        public override void OnGameLost()
        {
            base.OnGameLost();
            ResetScore();
        }

        public override void OnRestart()
        {
            base.OnRestart();
            ResetScore();
        }

        private void ResetScore()
        {
            if (PhotonNetwork.isMasterClient)
            {
                FengGameManagerMKII.instance.photonView.RPC("RefreshCaptureScore", PhotonTargets.Others, PvpHumanScoreLimit, PvpTitanScoreLimit);
            }
        }

        private GameObject spawnTitanRaw(Vector3 position, Quaternion rotation)
        {
            if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
            {
                return (GameObject)UnityEngine.Object.Instantiate(Resources.Load("TITAN_VER3.1"), position, rotation);
            }
            return PhotonNetwork.Instantiate("TITAN_VER3.1", position, rotation, 0);
        }

        public override void OnPlayerKilled(int id)
        {
            if (id != 0)
            {
                PvpTitanScore += 2;
            }
            CheckWinConditions();
        }
    }
}
