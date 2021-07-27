using Assets.Scripts.Characters.Humans.Equipment;
using Assets.Scripts.Characters.Titan;
using Assets.Scripts.Services;
using Assets.Scripts.Services.Interface;
using Assets.Scripts.Settings;
using ExitGames.Client.Photon;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.InGame
{
    [Obsolete("Use SpawnMenuV2 instead. Once this class can be deleted, rename the SpawnMenuV2 to SpawnMenu")]
    public class SpawnMenu : UiMenu
    {
        private ISpawnService SpawnService => Service.Spawn;

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

            if (!GameSettings.Gamemode.IsPlayerTitanEnabled.Value)
            {
                PlayerTitanButton.SetActive(false);
            }
        }

        /// <summary>
        /// The complex and confusing way of how AoTTG determined when the spawn a character
        /// </summary>
        public void Spawn()
        {
            string selection = "23";
            GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().needChooseSide = false;
            //if (!PhotonNetwork.isMasterClient && (GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().roundTime > 60f))
            //{
            //    if (!isPlayerAllDead2())
            //    {
            //        GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().NOTSpawnPlayer(selection);
            //    }
            //    else
            //    {
            //        GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().NOTSpawnPlayer(selection);
            //    }
            //}
            if (((GameSettings.Gamemode.GamemodeType == GamemodeType.TitanRush) || (GameSettings.Gamemode.GamemodeType == GamemodeType.Trost)) || GameSettings.Gamemode.GamemodeType == GamemodeType.Capture)
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
            Hashtable hashtable = new Hashtable();
            hashtable.Add(PhotonPlayerProperty.character, selection);
            Hashtable propertiesToSet = hashtable;
            PhotonNetwork.player.SetCustomProperties(propertiesToSet);
            gameObject.SetActive(false);
        }

        public void SpawnPlayerTitan()
        {
            GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().needChooseSide = false;
            SpawnService.Spawn<PlayerTitan>();
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
    }
}
