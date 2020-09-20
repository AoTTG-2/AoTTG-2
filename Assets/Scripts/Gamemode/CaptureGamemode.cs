using Assets.Scripts.Characters.Titan;
using Assets.Scripts.Characters.Titan.Behavior;
using Assets.Scripts.Settings;
using Assets.Scripts.Settings.Gamemodes;
using UnityEngine;

namespace Assets.Scripts.Gamemode
{
    public class CaptureGamemode : GamemodeBase
    {
        private CaptureGamemodeSettings Settings => GameSettings.Gamemode as CaptureGamemodeSettings;

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
            return $"{Settings.PvpTitanScoreLimit - PvpTitanScore} {str2} {Settings.PvpHumanScoreLimit - PvpHumanScore} \nTime : {length}";
        }

        public void SpawnCheckpointTitan(PVPcheckPoint target, Vector3 position, Quaternion rotation)
        {
            var configuration = GetTitanConfiguration();
            configuration.Behaviors.Add(new CaptureBehavior(target));
            SpawnService.Spawn<MindlessTitan>(position, rotation, configuration);
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
                photonView.RPC("RefreshCaptureScore", PhotonTargets.Others, HumanScore, TitanScore);
            }

            if (PvpTitanScore >= Settings.PvpTitanScoreLimit)
            {
                PvpTitanScore = Settings.PvpTitanScoreLimit.Value;
                FengGameManagerMKII.instance.gameLose2();
            }
            else if (PvpHumanScore >= Settings.PvpHumanScoreLimit)
            {
                PvpHumanScore = Settings.PvpHumanScoreLimit.Value;
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

        public override void OnLevelLoaded(Level level, bool isMasterClient = false)
        {
            base.OnLevelLoaded(level, isMasterClient);
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
                GameObject[] respawns = GameObject.FindGameObjectsWithTag("titanRespawn");
                if (respawns.Length <= 0)
                {
                    return;
                }
                foreach (var respawn in respawns)
                {
                    var configuration = GetTitanConfiguration(MindlessTitanType.Crawler);
                    SpawnService.Spawn<MindlessTitan>(respawn.transform.position, respawn.transform.rotation, configuration);
                }
            }
        }
        
        public override GameObject GetPlayerSpawnLocation(string tag = "playerRespawn")
        {
            if (FengGameManagerMKII.instance.checkpoint == null)
            {
                switch (tag)
                {
                    case "playerRespawn":
                        FengGameManagerMKII.instance.checkpoint = GameObject.Find("CheckpointStartHuman");
                        break;
                    case "titanRespawn":
                        FengGameManagerMKII.instance.checkpoint = GameObject.Find("CheckpointStartTitan");
                        break;
                }
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
                FengGameManagerMKII.instance.photonView.RPC(nameof(RefreshCaptureScore), PhotonTargets.Others, Settings.PvpHumanScoreLimit.Value, Settings.PvpTitanScoreLimit.Value);
            }
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
