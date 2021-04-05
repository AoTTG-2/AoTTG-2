using Assets.Scripts.CustomMaps;
using Assets.Scripts.Settings.Gamemodes;
using System.Collections.Generic;
using Assets.Scripts.Events;
using Assets.Scripts.Services;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Room
{
    public class Level
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string SceneName { get; set; }
        public CustomMapType Type { get; set; }
        public string AssetBundle { get; set; }
        public List<GamemodeSettings> Gamemodes { get; set; }

        public bool IsCustom => Type != CustomMapType.None;

        public event OnLevelLoaded OnLevelLoaded;

        public void LoadLevel()
        {
            if (Type == CustomMapType.CustomMap)
            {
                Service.Map.Load(Name);
            }
            else
            {

            }
        }

        public void LevelIsLoaded()
        {
            OnLevelLoaded?.Invoke();
        }
    }
}
