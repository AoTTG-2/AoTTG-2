using Assets.Scripts.Settings;
using Assets.Scripts.Settings.Gamemodes;
using UnityEngine;

namespace Assets.Scripts.Gamemode
{
    public class TrostGamemode : GamemodeBase
    {
        private TrostSettings Settings => GameSettings.Gamemode as TrostSettings;

        public override void OnLevelLoaded(Level level, bool isMasterClient = false)
        {
            base.OnLevelLoaded(level, isMasterClient);
            GameObject.Find("playerRespawn").SetActive(false);
            Object.Destroy(GameObject.Find("playerRespawn"));
            GameObject.Find("rock").GetComponent<Animation>()["lift"].speed = 0f;
            GameObject.Find("door_fine").SetActive(false);
            GameObject.Find("door_broke").SetActive(true);
            Object.Destroy(GameObject.Find("ppl"));

            if (!isMasterClient) return;
            //if (IsAllPlayersDead()) return;
        
            PhotonNetwork.Instantiate("TITAN_EREN_trost", new Vector3(-200f, 0f, -194f), Quaternion.Euler(0f, 180f, 0f), 0).GetComponent<ErenTitan>().rockLift = true;
            var obj4 = GameObject.Find("titanRespawnTrost");
            if (obj4 == null) return;

            var objArray2 = GameObject.FindGameObjectsWithTag("titanRespawn");
            foreach (GameObject obj5 in objArray2)
            {
                if (obj5.transform.parent.gameObject == obj4)
                {
                    FengGameManagerMKII.instance.SpawnTitan(obj5.transform.position, obj5.transform.rotation);
                }
            }
        }
    }
}
