using Assets.Scripts.Characters.Titan;
using Assets.Scripts.Room;
using Assets.Scripts.Settings;
using Assets.Scripts.Settings.Gamemodes;
using UnityEngine;

namespace Assets.Scripts.Gamemode
{
    public class TrostGamemode : GamemodeBase
    {
        public override GamemodeType GamemodeType { get; } = GamemodeType.Trost;
        private TrostSettings Settings => GameSettings.Gamemode as TrostSettings;

        protected override void Level_OnLevelLoaded(int scene, Level level)
        {
            base.Level_OnLevelLoaded(scene, level);
            GameObject.Find("playerRespawn").SetActive(false);
            Object.Destroy(GameObject.Find("playerRespawn"));
            GameObject.Find("rock").GetComponent<Animation>()["lift"].speed = 0f;
            GameObject.Find("door_fine").SetActive(false);
            GameObject.Find("door_broke").SetActive(true);
            Object.Destroy(GameObject.Find("ppl"));

            if (!PhotonNetwork.isMasterClient) return;

            var eren = SpawnService.Spawn<ErenTitan>(new Vector3(-200f, 0f, -194f), Quaternion.Euler(0f, 180f, 0f), null);
            eren.rockLift = true;
            var obj4 = GameObject.Find("titanRespawnTrost");
            if (obj4 == null) return;

            var objArray2 = GameObject.FindGameObjectsWithTag("titanRespawn");
            foreach (GameObject obj5 in objArray2)
            {
                if (obj5.transform.parent.gameObject == obj4)
                {
                    SpawnService.Spawn<MindlessTitan>(obj5.transform.position, obj5.transform.rotation, GetTitanConfiguration());
                }
            }
        }
    }
}
