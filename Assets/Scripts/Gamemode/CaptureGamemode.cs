using Assets.Scripts.Characters;
using Assets.Scripts.Characters.Humans;
using Assets.Scripts.Characters.Titan;
using Assets.Scripts.Characters.Titan.Behavior;
using Assets.Scripts.Room;
using Assets.Scripts.Settings;
using Assets.Scripts.Settings.Gamemodes;
using Assets.Scripts.UI.InGame.HUD;
using UnityEngine;

namespace Assets.Scripts.Gamemode
{
    public class CaptureGamemode : GamemodeBase
    {
        public override GamemodeType GamemodeType { get; } = GamemodeType.Capture;
        private CaptureGamemodeSettings Settings => GameSettings.Gamemode as CaptureGamemodeSettings;

        public int PvpTitanScore { get; set; }
        public int PvpHumanScore { get; set; }

        private const string HumanStart = "CheckpointStartHuman";
        private const string TitanStart = "CheckpointStartTitan";

        protected override void SetStatusTop()
        {
            var content = "| ";
            foreach (PVPcheckPoint checkpoint in PVPcheckPoint.chkPts)
            {
                content = content + checkpoint.getStateString() + " ";
            }
            content = $"| {Settings.PvpTitanScoreLimit - PvpTitanScore} {content} {Settings.PvpHumanScoreLimit - PvpHumanScore} \n" +
                      $"Time : {TimeService.GetRoundDisplayTime()}";

            UiService.SetMessage(LabelPosition.Top, content);
        }
        
        public void SpawnCheckpointTitan(PVPcheckPoint target, Vector3 position, Quaternion rotation)
        {
            var configuration = GetTitanConfiguration();
            configuration.Behaviors.Add(new CaptureBehavior(target));
            SpawnService.Spawn<MindlessTitan>(position, rotation, configuration);
        }

        protected override void OnEntityUnRegistered(Entity entity)
        {
            //TODO: Support factions!
            if (entity is MindlessTitan titan)
            {
                PvpHumanScore += 2;
                //switch (titanName)
                //{
                //    case "Titan":
                //        PvpHumanScore++;
                //        break;
                //    case "Aberrant":
                //        PvpHumanScore += 2;
                //        break;
                //    case "Jumper":
                //        PvpHumanScore += 3;
                //        break;
                //    case "Crawler":
                //        PvpHumanScore += 4;
                //        break;
                //    case "Female Titan":
                //        PvpHumanScore += 10;
                //        break;
                //    default:
                //        PvpHumanScore += 3;
                //        break;
                //}
            } else if (entity is Human human)
            {
                PvpTitanScore += 2;
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
                photonView.RPC(nameof(RefreshCaptureScore), PhotonTargets.Others, PvpHumanScore, PvpTitanScore);
            }

            string winner = null;
            if (PvpTitanScore >= Settings.PvpTitanScoreLimit)
            {
                TitanScore++;
                winner = "Titanity";
            }
            else if (PvpHumanScore >= Settings.PvpHumanScoreLimit)
            {
                HumanScore++;
                winner = "Humanity";
            }

            if (winner != null && PhotonNetwork.isMasterClient)
            {
                photonView.RPC(nameof(OnGameEndRpc), PhotonTargets.All, $"{winner} has won!\nRestarting in {{0}}s", HumanScore, TitanScore);
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

        protected override void Level_OnLevelLoaded(int scene, Level level)
        {
            base.Level_OnLevelLoaded(scene, level);
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

            if (PhotonNetwork.isMasterClient && FengGameManagerMKII.Level.SceneName == "OutSide")
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
                FengGameManagerMKII.instance.checkpoint = tag switch
                {
                    "playerRespawn" => GameObject.Find("CheckpointStartHuman"),
                    "titanRespawn" => GameObject.Find("CheckpointStartTitan"),
                    _ => null
                };
            }
            return FengGameManagerMKII.instance.checkpoint;
        }

        protected override void OnEntityRegistered(Entity entity)
        {
            if (entity is Hero)
            {
                entity.transform.position += new Vector3(Random.Range(-20, 20), 2f, Random.Range(-20, 20));
            }
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
                photonView.RPC(nameof(RefreshCaptureScore), PhotonTargets.All, 0, 0);
            }
        }
    }
}
