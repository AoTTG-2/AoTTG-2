using Assets.Scripts.Gamemode.Settings;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.InGame
{
    public class ServerSettingsPage : MonoBehaviour
    {
        public Dropdown LevelDropdown;
        public Dropdown GamemodeDropdown;
        private List<Level> levels;
        
        private Level selectedLevel;
        private GamemodeSettings selectedGamemode;

        TimeSwitcher DayNightCycle;
        
        private void Awake()
        {
            levels = LevelBuilder.GetAllLevels();
        }

        public void Start()
        {
            DayNightCycle = GameObject.Find("DayNightCycle").GetComponent<TimeSwitcher>();
            
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
            DayNightCycle.UpdateTime();
            FengGameManagerMKII.NewRoundGamemode = selectedGamemode;
            FengGameManagerMKII.NewRoundLevel = selectedLevel;
            FengGameManagerMKII.instance.photonView.RPC("Chat", PhotonTargets.All, $"Next round: {selectedLevel.Name}, with gamemode {selectedGamemode.GamemodeType}", string.Empty);
        }
    }
}
