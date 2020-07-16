using Assets.Scripts.Gamemode.Settings;
using Assets.Scripts.Room;
using ExitGames.Client.Photon;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Menu
{
    public class CreateRoom : UiNavigationElement
    {
        public Dropdown LevelDropdown;
        public Dropdown CustomLevelDropdown;
        public Dropdown GamemodeDropdown;

        public InputField RoomName;
        public InputField RoomPassword;

        private List<Level> levels = LevelBuilder.GetAllLevels();

        private Level selectedLevel;
        private GamemodeSettings selectedGamemode;
        private string SelectedCustomLevel { get; set; } = "None";
        
        public void Start()
        {
            LevelDropdown.options = new List<Dropdown.OptionData>();
            foreach (var level in levels)
            {
                LevelDropdown.options.Add(new Dropdown.OptionData(level.Name));
            }
            LevelDropdown.captionText.text = LevelDropdown.options[0].text;
            LevelDropdown.onValueChanged.AddListener(delegate
            {
                var level = levels.Single(x => x.Name == LevelDropdown.captionText.text);
                OnLevelSelected(level);
            });
            GamemodeDropdown.onValueChanged.AddListener(delegate
            {
                var gamemode = selectedLevel.Gamemodes.Single(x => x.Name == GamemodeDropdown.captionText.text 
                                                                   || x.GamemodeType.ToString() == GamemodeDropdown.captionText.text);
                OnGamemodeSelected(gamemode);
            });

            OnLevelSelected(levels[0]);

            CustomLevelDropdown.options = new List<Dropdown.OptionData>();
            CustomLevelDropdown.options.Add(new Dropdown.OptionData("None"));;
            foreach (var level in CustomLevelHelper.GetAll())
            {
                CustomLevelDropdown.options.Add(new Dropdown.OptionData(level));
            }
            CustomLevelDropdown.captionText.text = CustomLevelDropdown.options[0].text;
            CustomLevelDropdown.onValueChanged.AddListener(delegate
            {
                SelectedCustomLevel = CustomLevelDropdown.captionText.text;
            });
        }

        public void Create()
        {
            var roomNameInput = RoomName.text.Trim();
            var roomName = string.IsNullOrEmpty(roomNameInput)
                ? "FoodForTitans"
                : roomNameInput;

            PhotonNetwork.PhotonServerSettings.JoinLobby = false;
            var roomOptions = new RoomOptions
            {
                IsVisible = true,
                IsOpen = true,
                MaxPlayers = 10,
                CustomRoomProperties = new Hashtable
                {
                    { "name", roomName },
                    { "level", LevelDropdown.captionText.text },
                    { "gamemode", GamemodeDropdown.captionText.text }
                },
                CustomRoomPropertiesForLobby = new []{"name", "level", "gamemode"}
            };

            if (SelectedCustomLevel != "None")
            {
                roomOptions.CustomRoomProperties["level"] = SelectedCustomLevel;
            }

            var password = RoomPassword.text.Trim();
            if (!string.IsNullOrEmpty(password))
            {
                roomOptions.CustomRoomProperties.Add("password", password);
                roomOptions.CustomRoomProperties.Add("secure", true);

                var lobbyOptions = roomOptions.CustomRoomPropertiesForLobby.ToList();
                lobbyOptions.Add("secure");
                roomOptions.CustomRoomPropertiesForLobby = lobbyOptions.ToArray();
            }

            PhotonNetwork.PhotonServerSettings.JoinLobby = true;
            PhotonNetwork.CreateRoom(Guid.NewGuid().ToString(), roomOptions, TypedLobby.Default); 
            SceneManager.sceneLoaded += SceneLoaded;
        }

        private void OnLevelSelected(Level level)
        {
            selectedLevel = level;
            GamemodeDropdown.options = new List<Dropdown.OptionData>();
            foreach (var gamemode in level.Gamemodes)
            {
                GamemodeDropdown.options.Add(new Dropdown.OptionData(gamemode.Name ?? gamemode.GamemodeType.ToString()));
            }
            GamemodeDropdown.captionText.text = GamemodeDropdown.options[0].text;
        }

        private void OnGamemodeSelected(GamemodeSettings gamemode)
        {
            selectedGamemode = gamemode;
        }

        private void SceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            SceneManager.sceneLoaded -= SceneLoaded;
            Canvas.ShowInGameUi();
        }
    }
}