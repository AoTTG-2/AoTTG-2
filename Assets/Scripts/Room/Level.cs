using Assets.Scripts.CustomMaps;
using Assets.Scripts.Services;
using Assets.Scripts.Settings.Gamemodes;
using System.Collections.Generic;

namespace Assets.Scripts.Room
{
    /// <summary>
    /// Contains settings related to a Level.
    /// </summary>
    public class Level
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string SceneName { get; set; }
        public CustomMapType Type { get; set; }
        public string AssetBundle { get; set; }
        public List<GamemodeSettings> Gamemodes { get; set; }

        public bool IsCustom => Type != CustomMapType.None;

        public void LoadLevel()
        {
            if (Type == CustomMapType.CustomMap)
            {
                Service.Map.Load(Name);
            }
            else
            {
                Service.Map.LoadScene(SceneName);
            }
        }
    }
}
