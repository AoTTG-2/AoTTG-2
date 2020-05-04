using UnityEngine;

namespace Assets.Scripts.Gamemode
{
    public class KillTitansGamemode : GamemodeBase
    {
        public KillTitansGamemode()
        {
            GamemodeType = GamemodeType.Titans;
            RestartOnTitansKilled = true;
            RespawnMode = RespawnMode.NEVER;
        }

        public override void OnAllTitansDead()
        {
            FengGameManagerMKII.instance.gameWin2();
            Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
        }

        public override void OnLevelLoaded(Level level, bool isMasterClient = false)
        {
            base.OnLevelLoaded(level, isMasterClient);
            if (!isMasterClient) return;

            if (Name.Contains("Annie"))
            {
                PhotonNetwork.Instantiate("FEMALE_TITAN", GameObject.Find("titanRespawn").transform.position, GameObject.Find("titanRespawn").transform.rotation, 0);
            }
            else
            {
                for (int i = 0; i < Titans; i++)
                {
                    FengGameManagerMKII.instance.SpawnTitan(GetTitanConfiguration());
                }
            }

        }
    }
}