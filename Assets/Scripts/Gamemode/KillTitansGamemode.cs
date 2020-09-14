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

        public override void OnLevelLoaded(Level level, bool isMasterClient = false)
        {
            base.OnLevelLoaded(level, isMasterClient);
            if (!isMasterClient) return;

            if (GameSettings.Gamemode.Name.Contains("Annie"))
            {
                var ftSpawn = GameObject.Find("titanRespawn").transform;
                SpawnService.Spawn<FemaleTitan>(ftSpawn.position, ftSpawn.rotation, new TitanConfiguration());
            }
            else
            {
                SpawnTitans(GameSettings.Titan.Start.Value);
            }
        }
    }
}