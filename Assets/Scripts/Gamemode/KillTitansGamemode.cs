using Assets.Scripts.Characters;
using Assets.Scripts.Characters.Titan.Configuration;
using Assets.Scripts.Room;
using Assets.Scripts.Settings;
using Assets.Scripts.Settings.Gamemodes;
using UnityEngine;

namespace Assets.Scripts.Gamemode
{
    public class KillTitansGamemode : GamemodeBase
    {
        public override GamemodeType GamemodeType { get; } = GamemodeType.Titans;
        private KillTitansSettings Settings => GameSettings.Gamemode as KillTitansSettings;

        protected override void OnFactionDefeated(Faction faction)
        {
            if (IsRoundOver || !PhotonNetwork.isMasterClient) return;
            string winner;
            if (faction == FactionService.GetHumanity())
            {
                TitanScore++;
                winner = "Titanity";
            }
            else
            {
                HumanScore++;
                winner = "Humanity";
            }

            photonView.RPC(nameof(OnGameEndRpc), PhotonTargets.All, $"{winner} has won!\nRestarting in {{0}}s", HumanScore, TitanScore);
            Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
        }

        protected override void Level_OnLevelLoaded(int scene, Level level)
        {
            base.Level_OnLevelLoaded(scene, level);
            if (!PhotonNetwork.isMasterClient) return;
            if (GameSettings.Gamemode.Name.Contains("Annie"))
            {
                var ftSpawn = GameObject.Find("titanRespawn").transform;
                SpawnService.Spawn<FemaleTitan>(ftSpawn.position, ftSpawn.rotation, new TitanConfiguration());
            }
            //TODO: 160 Experimentation
            //else if (GameSettings.Gamemode.Name.Contains("Test") || true)
            //{
            //    var spawns = GameObject.FindGameObjectsWithTag("titanRespawn");
            //    for (var i = 0; i < 1; i++)
            //    {
            //        var randomSpawn = spawns[Random.Range(0, spawns.Length)].transform;
            //        var et = SpawnService.Spawn<ErenTitan>(randomSpawn.position, new Quaternion(),
            //            new TitanConfiguration());
            //        et.Faction = FactionService.GetHumanity();
            //    }

            //    //var et = SpawnService.Spawn<ErenTitan>(spawns[0].transform.position, new Quaternion(),
            //    //    new TitanConfiguration());
            //    //et.Faction = FactionService.GetHumanity();

            //    for (var i = 0; i < 1; i++)
            //    {
            //        var randomSpawn = spawns[Random.Range(0, spawns.Length)].transform;
            //        SpawnService.Spawn<MindlessTitan>(randomSpawn.position, randomSpawn.rotation, new TitanConfiguration());
            //    }

            //    //var tit1 = SpawnService.Spawn<MindlessTitan>(spawns[4].transform.position, spawns[0].transform.rotation, new TitanConfiguration());
            //    //var tit2 = SpawnService.Spawn<MindlessTitan>(spawns[0].transform.position, spawns[1].transform.rotation, new TitanConfiguration());
            //    //tit1.Faction = FactionService.GetHumanity();
            //    //tit2.Faction = FactionService.GetTitanity();

            //    //for (int i = 0; i < 15; i++)
            //    //{
            //    //    var tit3 = SpawnService.Spawn<MindlessTitan>(ftSpawn[0].transform.position, ftSpawn[0].transform.rotation, new TitanConfiguration());
            //    //    tit3.Faction = FactionService.GetHumanity();
            //    //}
            //}
            else
            {
                SpawnTitans(GameSettings.Titan.Start.Value);
            }
        }
    }
}