using UnityEngine;

namespace Assets.Scripts.Gamemode
{
    public class TrostGamemode : GamemodeBase
    {
        public TrostGamemode()
        {
            GamemodeType = GamemodeType.Trost;
            PlayerTitanShifters = false;
            Titans = 2;
            Punks = false;
        }

        public override void OnLevelWasLoaded(Level level, bool isMasterClient = false)
        {
            base.OnLevelWasLoaded(level, isMasterClient);
            GameObject.Find("playerRespawn").SetActive(false);
            Object.Destroy(GameObject.Find("playerRespawn"));
            GameObject.Find("rock").GetComponent<Animation>()["lift"].speed = 0f;
            GameObject.Find("door_fine").SetActive(false);
            GameObject.Find("door_broke").SetActive(true);
            Object.Destroy(GameObject.Find("ppl"));

            if (!isMasterClient) return;
            //if (IsAllPlayersDead()) return;
        
            PhotonNetwork.Instantiate("TITAN_EREN_trost", new Vector3(-200f, 0f, -194f), Quaternion.Euler(0f, 180f, 0f), 0).GetComponent<TITAN_EREN>().rockLift = true;
            var obj4 = GameObject.Find("titanRespawnTrost");
            if (obj4 == null) return;

            var rate = 90;
            if (Difficulty == 1)
            {
                rate = 70;
            }
            var objArray2 = GameObject.FindGameObjectsWithTag("titanRespawn");
            foreach (GameObject obj5 in objArray2)
            {
                if (obj5.transform.parent.gameObject == obj4)
                {
                    FengGameManagerMKII.instance.spawnTitan(rate, obj5.transform.position, obj5.transform.rotation, false);
                }
            }
        }
    }
}
