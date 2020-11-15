using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Characters.Humans.Customization
{
    public class CustomizationManager : MonoBehaviour
    {
        public CharacterPrefabs Prefabs;
        public List<CharacterPreset> Presets;

        public async void Start()
        {
            var human = GameObject.Find("Example").GetComponent<Human>();
            if (human == null) return;
            await Presets[0].Apply(human, Prefabs);
        }
    }
}
