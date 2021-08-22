using Assets.Scripts.CustomMaps;
using Assets.Scripts.Settings.New.Gamemodes;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Room
{
    [CreateAssetMenu(fileName = "Level", menuName = "Settings/Level", order = 1)]
    public sealed class Level : ScriptableObject
    {
        public string Name;
        public string Description;
        public string SceneName;
        public List<GamemodeSetting> Gamemodes;
        
        public bool IsCustom => Type == null || Type == CustomMapType.None;
        public CustomMapType? Type { get; private set; }
        public string AssetBundle { get; private set; }
    }
}
