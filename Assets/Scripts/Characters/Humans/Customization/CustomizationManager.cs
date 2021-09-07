using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Characters.Humans.Customization
{
    /// <summary>
    /// Used to test the Character Customization functionality in a scene
    /// </summary>
    public class CustomizationManager : MonoBehaviour
    {
        public CharacterPrefabs Prefabs;
        public List<CharacterPreset> Presets;

        public void Start()
        {
            var human = GameObject.Find("Example")?.GetComponent<Human>();
            if (human == null) return;
            Presets[0].Apply(human, Prefabs);
        }
    }
}
