using Assets.Scripts.Gamemode;
using Assets.Scripts.Room;
using Assets.Scripts.Services;
using Assets.Scripts.Settings.Gamemodes;
using ExitGames.Client.Photon;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Menu
{
    /// <summary>
    /// UI Class for the "Create Room" functionality
    /// </summary>
    public class CreateRoom : UiNavigationElement
    {
        public Dropdown LevelDropdown;
        public Dropdown GamemodeDropdown;
        public Dropdown DifficultyDropdown;

        public InputField RoomName;
        public InputField RoomPassword;

        public Toggle Account;

        private List<Level> levels;

        private Level selectedLevel;
        private GamemodeSettings selectedGamemode;
        private Dictionary<string, string> CustomDifficulties { get; } = new Dictionary<string, string>();
        private const string CustomDifficultyPrefix = "*-";

        private void Awake()
        {
            levels = LevelBuilder.GetAllLevels();
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            Refresh();

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

            DifficultyDropdown.options = new List<Dropdown.OptionData>();
            foreach (Difficulty difficulty in Enum.GetValues(typeof(Difficulty)))
            {
                DifficultyDropdown.options.Add(new Dropdown.OptionData(difficulty.ToString()));
            }
            DifficultyDropdown.captionText.text = DifficultyDropdown.options[0].text;

            var files = Directory.GetFiles(Application.streamingAssetsPath + Path.DirectorySeparatorChar + "Difficulty", "*.json");
            foreach (var file in files)
            {
                var fileName = file.Split(Path.DirectorySeparatorChar).Last().Replace(".json", string.Empty);
                CustomDifficulties.Add(fileName, file);
                DifficultyDropdown.options.Add(new Dropdown.OptionData($"{CustomDifficultyPrefix}{fileName}"));
            }
        }

        private void Refresh()
        {
            CustomDifficulties.Clear();
            LevelDropdown.value = 0;
            GamemodeDropdown.value = 0;
            DifficultyDropdown.value = 0;
            Account.isOn = false;
        }

        /// <summary>
        /// Creates a new multiplayer room with the configured settings.
        /// </summary>
        public void Create()
        {
            if (DifficultyDropdown.captionText.text.StartsWith(CustomDifficultyPrefix))
            {
                var customDifficulty = DifficultyDropdown.captionText.text.Replace(CustomDifficultyPrefix, string.Empty);
                using (var reader = File.OpenText(CustomDifficulties[customDifficulty]))
                {
                    Service.Settings.SyncSettings(reader.ReadToEnd());
                }
            }
            else
            {
                var difficulty = (Difficulty) DifficultyDropdown.value;
                Service.Settings.SyncSettings(difficulty);
            }

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
                PlayerTtl = 120000,
                CustomRoomProperties = new Hashtable
                {
                    { "name", roomName },
                    { "level", LevelDropdown.captionText.text },
                    { "gamemode", GamemodeDropdown.captionText.text }
                },
                CustomRoomPropertiesForLobby = new[] { "name", "level", "gamemode" }
            };

            var password = RoomPassword.text.Trim();
            if (!string.IsNullOrEmpty(password))
            {
                roomOptions.CustomRoomProperties.Add("password", password);
                roomOptions.CustomRoomProperties.Add("passworded", true);

                var lobbyOptions = roomOptions.CustomRoomPropertiesForLobby.ToList();
                lobbyOptions.Add("passworded");
                roomOptions.CustomRoomPropertiesForLobby = lobbyOptions.ToArray();
            }

            if (Account.isOn)
            {
                roomOptions.CustomRoomProperties.Add("account", true);
                var lobbyOptions = roomOptions.CustomRoomPropertiesForLobby.ToList();
                lobbyOptions.Add("account");
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
