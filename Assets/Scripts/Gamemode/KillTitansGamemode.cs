using Assets.Scripts.Gamemode.Settings;
using UnityEngine;

namespace Assets.Scripts.Gamemode
{
    public class KillTitansGamemode : GamemodeBase
    {
        public sealed override GamemodeSettings Settings { get; set; }
        private KillTitansSettings GamemodeSettings => Settings as KillTitansSettings;

        public override void OnAllTitansDead()
        {
            FengGameManagerMKII.instance.gameWin2();
            Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
        }

        public override void OnLevelLoaded(Level level, bool isMasterClient = false)
        {
            base.OnLevelLoaded(level, isMasterClient);
            if (!isMasterClient) return;

            if (GamemodeSettings.Name.Contains("Annie"))
            {
                PhotonNetwork.Instantiate("FEMALE_TITAN", GameObject.Find("titanRespawn").transform.position, GameObject.Find("titanRespawn").transform.rotation, 0);
            }
            else
            {
                for (int i = 0; i < Settings.Titans; i++)
                {
                    FengGameManagerMKII.instance.SpawnTitan(GetTitanConfiguration());
                }
            }

        }
    }
}