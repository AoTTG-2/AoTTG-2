using System.Threading;
using UnityEngine;
using ExitGames.Client.Photon;

namespace Assets.Scripts.UI
{
    public static class AottgUi
    {
        public static void Init(FengGameManagerMKII manager)
        {
            GUI.backgroundColor = new Color(0f, 0f, 0f, 1f);
            float left = (Screen.width / 2) - 115f;
            float top = (Screen.height / 2) - 45f;
            //GUI.Box(new Rect(left, top, 230f, 90f), string.Empty);
            //GUI.DrawTexture(new Rect(left + 2f, top + 2f, 226f, 86f), manager.textureBackgroundBlack);
            if (GUI.Button(new Rect(left + 13f, top - 20f, 172f, 70f), "City"))
            {
                FengGameManagerMKII.settings[0x40] = 0x65;
                Application.LoadLevel("The City I");
            }

            if (GUI.Button(new Rect(left + 13f, top - 70f, 172f, 70f), "MultiPlayer"))
            {
                //PhotonNetwork.ConnectToMaster("app-eu.exitgamescloud.com", 0x13bf, FengGameManagerMKII.applicationId, UIMainReferences.version);
                PhotonNetwork.CreateRoom("Test", true, true, 1);
            }

            if (GUI.Button(new Rect(left + 13f, top - 120f, 172f, 70f), "Spawn"))
            {
                SpawnHuman();
            }
        }

        private static bool isPlayerAllDead2()
        {
            int num = 0;
            int num2 = 0;
            foreach (PhotonPlayer player in PhotonNetwork.playerList)
            {
                if (RCextensions.returnIntFromObject(player.customProperties[PhotonPlayerProperty.isTitan]) == 1)
                {
                    num++;
                    if (RCextensions.returnBoolFromObject(player.customProperties[PhotonPlayerProperty.dead]))
                    {
                        num2++;
                    }
                }
            }
            return (num == num2);
        }

        private static void SpawnHuman()
        {
            //string selection = GameObject.Find("PopupListCharacterHUMAN").GetComponent<UIPopupList>().selection;
            //NGUITools.SetActive(GameObject.Find("UI_IN_GAME").GetComponent<UIReferArray>().panels[0], true);
            string selection = null;
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
            //NGUITools.SetActive(GameObject.Find("UI_IN_GAME").GetComponent<UIReferArray>().panels[1], false);
            //NGUITools.SetActive(GameObject.Find("UI_IN_GAME").GetComponent<UIReferArray>().panels[2], false);
            //NGUITools.SetActive(GameObject.Find("UI_IN_GAME").GetComponent<UIReferArray>().panels[3], false);
            IN_GAME_MAIN_CAMERA.usingTitan = false;
            GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().setHUDposition();
            Hashtable hashtable = new Hashtable();
            hashtable.Add(PhotonPlayerProperty.character, selection);
            Hashtable propertiesToSet = hashtable;
            PhotonNetwork.player.SetCustomProperties(propertiesToSet);
        }
    }
}
