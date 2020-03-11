using ExitGames.Client.Photon;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class AottgUi:MonoBehaviour
    {
        public void Start()
        {
            UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
        }

        public static void TestSpawn()
        {
            SpawnHuman();
            "The Hero has been spawned!".SendProcessing(true);
        }

        public void OnGUI()
        {
            if (!FengGameManagerMKII.showHackMenu) return;

            GUI.backgroundColor = new Color(0f, 0f, 0f, 1f);
            float left = (Screen.width / 2) - 115f;
            float top = (Screen.height / 2) - 45f;
            if (GUI.Button(new Rect(left + 13f, top - 120f, 172f, 70f), "Load/spawn..."))
            {
                FengGameManagerMKII.instance.StartCoroutine(EMCli.ConnectAndJoinIE(true));
                FengGameManagerMKII.showHackMenu = false;
            }
        }

        private static bool isPlayerAllDead2()
        {
            int num = 0;
            int num2 = 0;
            foreach (PhotonPlayer player in PhotonNetwork.playerList)
            {
                if (RCextensions.returnIntFromObject(player.CustomProperties[PhotonPlayerProperty.isTitan]) == 1)
                {
                    num++;
                    if (RCextensions.returnBoolFromObject(player.CustomProperties[PhotonPlayerProperty.dead]))
                    {
                        num2++;
                    }
                }
            }
            return (num == num2);
        }

        private static void SpawnHuman()
        {
            string selection = "23";
            GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().needChooseSide = false;
            if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.PVP_CAPTURE)
            {
                GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().checkpoint = GameObject.Find("PVPchkPtH");
            }
            if (!PhotonNetwork.isMasterClient && (GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().roundTime > 60f))
            {
                if (!isPlayerAllDead2())
                {
                    GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().NOTSpawnPlayer(selection);
                }
                else
                {
                    GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().NOTSpawnPlayer(selection);
                    GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().photonView.RPC("restartGameByClient", PhotonTargets.MasterClient, new object[0]);
                }
            }
            else if (((IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.BOSS_FIGHT_CT) || (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.TROST)) || (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.PVP_CAPTURE))
            {
                if (isPlayerAllDead2())
                {
                    GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().NOTSpawnPlayer(selection);
                    GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().photonView.RPC("restartGameByClient", PhotonTargets.MasterClient, new object[0]);
                }
                else
                {
                    GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().SpawnPlayer(selection, "playerRespawn");
                }
            }
            else
            {
                GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().SpawnPlayer(selection, "playerRespawn");
            }
            IN_GAME_MAIN_CAMERA.usingTitan = false;
            GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().setHUDposition();
            Hashtable hashtable = new Hashtable();
            hashtable.Add(PhotonPlayerProperty.character, selection);
            Hashtable propertiesToSet = hashtable;
            PhotonNetwork.player.SetCustomProperties(propertiesToSet);
        }
    }
}
