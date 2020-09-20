using Assets.Scripts.Characters;
using Assets.Scripts.Characters.Titan;
using Assets.Scripts.Characters.Titan.Configuration;
using Assets.Scripts.Settings;
using Assets.Scripts.Settings.Gamemodes;
using UnityEngine;

namespace Assets.Scripts.Gamemode
{
    public class KillTitansGamemode : GamemodeBase
    {
        private KillTitansSettings Settings => GameSettings.Gamemode as KillTitansSettings;

        public override void OnAllTitansDead()
        {
            FengGameManagerMKII.instance.gameWin2();
            Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
        }

        protected override void OnFactionDefeated(Faction faction)
        {
            if (faction == FactionService.GetHumanity())
            {
                TitanScore++;
                //FengGameManagerMKII.instance.gameLose2();
            }
            else
            {
                HumanScore++;
                //FengGameManagerMKII.instance.gameWin2();
            }

            photonView.RPC(nameof(OnGameEndRpc), PhotonTargets.All, "Round Ended Message", HumanScore, TitanScore);
            Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
        }

        public override void OnLevelLoaded(Level level, bool isMasterClient = false)
        {
            base.OnLevelLoaded(level, isMasterClient);
            if (!isMasterClient) return;

            if (GameSettings.Gamemode.Name.Contains("Annie"))
            {
                var ftSpawn = GameObject.Find("titanRespawn").transform;
                SpawnService.Spawn<FemaleTitan>(ftSpawn.position, ftSpawn.rotation, new TitanConfiguration());
            }
            else if (GameSettings.Gamemode.Name.Contains("Test"))
            {
                var ftSpawn = GameObject.FindGameObjectsWithTag("titanRespawn");
                var tit1 = SpawnService.Spawn<MindlessTitan>(ftSpawn[1].transform.position, ftSpawn[0].transform.rotation, new TitanConfiguration());
                var tit2 = SpawnService.Spawn<MindlessTitan>(ftSpawn[0].transform.position, ftSpawn[1].transform.rotation, new TitanConfiguration());

                tit1.Faction = FactionService.GetHumanity();
                tit2.Faction = FactionService.GetTitanity();

                //for (int i = 0; i < 15; i++)
                //{
                //    var tit3 = SpawnService.Spawn<MindlessTitan>(ftSpawn[0].transform.position, ftSpawn[0].transform.rotation, new TitanConfiguration());
                //    tit3.Faction = FactionService.GetHumanity();
                //}

                //for (int i = 0; i < 25; i++)
                //{
                //    var tit3 = SpawnService.Spawn<MindlessTitan>(ftSpawn[0].transform.position, ftSpawn[0].transform.rotation, new TitanConfiguration());
                //    tit3.Faction = FactionService.GetTitanity();
                //}
            }
            else
            {
                SpawnTitans(GameSettings.Titan.Start.Value);
            }
        }
    }
}