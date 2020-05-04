using UnityEngine;

namespace Assets.Scripts.Gamemode
{
    //This is the colossal gamemode, where titans "rush" towards a specified endpoint
    public class TitanRushGamemode : GamemodeBase
    {
        public TitanRushGamemode()
        {
            GamemodeType = GamemodeType.TitanRush;
            Titans = 2;
        }

        public override void OnLevelWasLoaded(Level level, bool isMasterClient = false)
        {
            base.OnLevelWasLoaded(level, isMasterClient);
            GameObject.Find("playerRespawnTrost").SetActive(false);
            Object.Destroy(GameObject.Find("playerRespawnTrost"));
            Object.Destroy(GameObject.Find("rock"));
            if (!isMasterClient) return;
            //if (IsAllPlayersDead()) return;
            PhotonNetwork.Instantiate("COLOSSAL_TITAN", (Vector3)(-Vector3.up * 10000f), Quaternion.Euler(0f, 180f, 0f), 0);
        }

        public override string GetGamemodeStatusTop(int time = 0, int totalRoomTime = 0)
        {
            var content = "Time : ";
            var length = time - totalRoomTime;
            return content + length.ToString() + "\nDefeat the Colossal Titan.\nPrevent abnormal titan from running to the north gate";
        }
    }
}
