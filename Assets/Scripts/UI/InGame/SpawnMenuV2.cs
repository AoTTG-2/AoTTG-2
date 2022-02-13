using Assets.Scripts.Characters.Humans;
using Assets.Scripts.Characters.Humans.Customization;
using Assets.Scripts.Characters.Titan;
using Assets.Scripts.Services;
using Assets.Scripts.Services.Interface;
using Assets.Scripts.Settings;
using Assets.Scripts.UI.Menu;
using Assets.Scripts.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using MonoBehaviour = Photon.MonoBehaviour;

namespace Assets.Scripts.UI.InGame
{
    /// <summary>
    /// The new SpawnMenu, used to select the preferred character or player titan.
    /// </summary>
    public class SpawnMenuV2 : MonoBehaviour, IUiContainer
    {
        private ISpawnService SpawnService => Service.Spawn;


        public GameObject mainWindow;

        [Space] public CharacterList CharacterList;
        public CharacterPrefabs Prefabs;
        public TMP_Dropdown CharacterDropdown;
        public TMP_Dropdown OutfitDropdown;
        public TMP_Dropdown BuildDropdown;

        /// <summary>
        /// The Area in the UI where the character model will be loaded. Currently this is unused
        /// </summary>
        public GameObject HeroLocation;
        private GameObject Character { get; set; }

        public void Awake()
        {
            RecreateCharacterDropdown();
            OnCharacterChanged(CharacterList.Characters.First(), 0);
            CharacterDropdown.onValueChanged.AddListener(x => OnCharacterChanged(CharacterList.Characters[x], 0));
        }

        public void OnEnable()
        {
            RecreateCharacterDropdown();
            OnCharacterChanged(CharacterList.Characters[CharacterDropdown.value], 0);
            MenuManager.RegisterOpened(this);
        }

        /// <summary>
        /// Clears the <see cref="CharacterDropdown"/> and initializes it again
        /// </summary>
        public void RecreateCharacterDropdown()
        {
            CharacterDropdown.ClearOptions();
            var options = CharacterList.Characters.Select(x => new TMP_Dropdown.OptionData
            {
                text = x.Name
            });
            CharacterDropdown.AddOptions(options.ToList());
        }

        public void OnDisable()
        {
            if (Character != null)
                Destroy(Character);

            MenuManager.RegisterClosed(this);
        }

        /// <summary>
        /// Spawns the player in the game as a <see cref="Hero"/>
        /// </summary>
        public void Spawn()
        {
            string selection = "23";
            var selectedPreset = CharacterList.Characters[CharacterDropdown.value];
            selectedPreset.CurrentOutfit = selectedPreset.CharacterOutfit[OutfitDropdown.value];
            selectedPreset.CurrentBuild = selectedPreset.CharacterBuild[BuildDropdown.value];

            GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().needChooseSide = false;
            GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().SpawnPlayer(selection, "playerRespawn", selectedPreset);
            if ((((GameSettings.Gamemode.GamemodeType == GamemodeType.TitanRush) || (GameSettings.Gamemode.GamemodeType == GamemodeType.Trost)) || GameSettings.Gamemode.GamemodeType == GamemodeType.Capture) && isPlayerAllDead2())
            {
                GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().NOTSpawnPlayer(selection);
            }

            IN_GAME_MAIN_CAMERA.usingTitan = false;
            Hashtable hashtable = new Hashtable();
            hashtable.Add(PhotonPlayerProperty.character, selection);
            Hashtable propertiesToSet = hashtable;
            PhotonNetwork.player.SetCustomProperties(propertiesToSet);
            gameObject.SetActive(false);

            SpectatorMode.Disable(); //Reset spectator mode
        }

        /// <summary>
        /// Spawns the player in the game as a <see cref="PlayerTitan"/>
        /// </summary>
        public void SpawnPlayerTitan()
        {
            GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().needChooseSide = false;
            SpawnService.Spawn<PlayerTitan>();
            gameObject.SetActive(false);
        }

        [Obsolete("Legacy way of determining if all human players are dead. Use FactionService instead for get team information")]
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

        private void OnCharacterChanged(CharacterPreset preset, int outfit)
        {
            SetDropdownOptions(preset);

            return;
            if (Character != null)
                Destroy(Character);

            var character = (GameObject) Instantiate(Resources.Load("Character2/HumanBase2"));
            character.transform.parent = HeroLocation.transform;
            var rigid = character.GetComponent<Rigidbody>();
            rigid.constraints = RigidbodyConstraints.FreezeAll;
            character.transform.position = new Vector3(0, 0, 0);
            character.transform.rotation = Quaternion.Euler(0, 180, 0);
            character.transform.localPosition = new Vector3(0, 0, 0);

            preset.Apply(character, Prefabs);

            character.transform.localScale = new Vector3(150f, 150f, 150f);
            character.GetComponentsInChildren<Renderer>().ToList()
                .ForEach(x => x.receiveShadows = false);
            Character = character;
        }

        private void SetDropdownOptions(CharacterPreset preset)
        {
            OutfitDropdown.ClearOptions();
            var options = preset.CharacterOutfit.Select(x => new TMP_Dropdown.OptionData
            {
                text = x.Name
            });
            OutfitDropdown.AddOptions(options.ToList());

            BuildDropdown.ClearOptions();
            options = preset.CharacterBuild.Select(x => new TMP_Dropdown.OptionData
            {
                text = x.Name
            });
            BuildDropdown.AddOptions(options.ToList());
        }

        private void Update()
        {
            if (Character == null) return;
            Character.transform.position = new Vector3(0, 0, 0);
            Character.transform.rotation = Quaternion.Euler(0, 180, 0);
            Character.transform.localPosition = new Vector3(0, 0, 0);
        }

        public List<IUiElement> GetChildren()
        {
            throw new System.NotImplementedException();
        }

        public int GetNumVisibleChildren()
        {
            throw new System.NotImplementedException();
        }

        public void AddChild(IUiElement element)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveChild(IUiElement element)
        {
            throw new System.NotImplementedException();
        }

        public bool IsVisible()
        {
            return mainWindow.activeSelf;
        }

        public void Show()
        {
            mainWindow.SetActive(true);
        }

        public void Hide()
        {
            mainWindow.SetActive(false);

        }
    }
}
