using Assets.Scripts.Services;
using Assets.Scripts.Settings.Gamemodes;
using Assets.Scripts.Room;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

namespace Assets.Scripts.UI.InGame
{
    /// <summary>
    /// Server Settings UI Page within the in-game settings overview
    /// </summary>
    public class ServerSettingsPage : UiContainer
    {
        public Dropdown LevelDropdown;
        public Dropdown GamemodeDropdown;
        private List<Level> levels;
        
        private Level selectedLevel;
        private GamemodeSettings selectedGamemode;

        
        
        private void Awake()
        {
            levels = LevelBuilder.GetAllLevels();
        }

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
            OnGamemodeSelected(level.Gamemodes[0]);
        }

        private void OnGamemodeSelected(GamemodeSettings gamemode)
        {
            selectedGamemode = gamemode;
        }
      
        public void Sync()
        {
            if (!PhotonNetwork.isMasterClient) return;
            Service.Settings.SyncSettings();
            FengGameManagerMKII.NewRoundGamemode = selectedGamemode;
            FengGameManagerMKII.NewRoundLevel = selectedLevel;
            FengGameManagerMKII.instance.photonView.RPC(nameof(FengGameManagerMKII.Chat), PhotonTargets.All, $"Next round: {selectedLevel.Name}, with gamemode {selectedGamemode.GamemodeType}", string.Empty);
        }
    }
}
