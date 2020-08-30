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
                PhotonNetwork.Instantiate("FemaleTitan", GameObject.Find("titanRespawn").transform.position, GameObject.Find("titanRespawn").transform.rotation, 0);
            }
            else
            {
                SpawnTitans(GameSettings.Titan.Start.Value);
            }
        }
    }
}