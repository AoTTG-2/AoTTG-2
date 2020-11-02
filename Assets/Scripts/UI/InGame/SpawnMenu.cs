using ExitGames.Client.Photon;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.InGame
{
    public class SpawnMenu : MonoBehaviour
    {
        public Dropdown EquipmentDropdown;
        public GameObject PlayerTitanButton;

        private void Start()
        {
            EquipmentDropdown.options = new List<Dropdown.OptionData>();
            foreach (var equipment in Enum.GetNames(typeof(EquipmentType)))
            {
                EquipmentDropdown.options.Add(new Dropdown.OptionData(equipment));
            }
            EquipmentDropdown.captionText.text = EquipmentDropdown.options[0].text;

            if (!FengGameManagerMKII.Gamemode.Settings.IsPlayerTitanEnabled)
            {
                PlayerTitanButton.SetActive(false);
            }
        }

        public void Spawn()
        {
            string selection = "23";
            GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().needChooseSide = false;
            if (!PhotonNetwork.isMasterClient && (GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().roundTime > 60f))
            {
                if (!isPlayerAllDead2())
                {
                    GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().NOTSpawnPlayer(selection);
                }
                else
                {
                    GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().NOTSpawnPlayer(selection);
                }
            }
            else if (((FengGameManagerMKII.Gamemode.Settings.GamemodeType == GamemodeType.TitanRush) || (FengGameManagerMKII.Gamemode.Settings.GamemodeType == GamemodeType.Trost)) || FengGameManagerMKII.Gamemode.Settings.GamemodeType == GamemodeType.Capture)
            {
                GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().SpawnPlayer(selection, "playerRespawn");
                if (isPlayerAllDead2())
                {
                    GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().NOTSpawnPlayer(selection);
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
            gameObject.SetActive(false);
        }

        public void SpawnPlayerTitan()
        {
            GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().needChooseSide = false;
            FengGameManagerMKII.instance.SpawnPlayerTitan();
            gameObject.SetActive(false);
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

        private void OnEnable()
        {
            MenuManager.RegisterOpened();
        }

        private void OnDisable()
        {
            MenuManager.RegisterClosed();
        }
    }
}
