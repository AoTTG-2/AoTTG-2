using System;
using Assets.Scripts.Characters.Humans.Customization.Components;
using Newtonsoft.Json;
using UnityEngine;

namespace Assets.Scripts.Characters.Humans.Customization
{
    public struct CustomizationNetworkObject
    {

        [SerializeField] public string Name;
        [SerializeField] public string Description;

        public int HeadPrefab;
        public int HeadTexture;

        public CharacterOutfit CurrentOutfit { get; set; }
        public CharacterBuild CurrentBuild { get; set; }

        public static CustomizationNetworkObject Convert(CharacterPrefabs prefabs, CharacterPreset preset)
        {
            //TODO: This is only required during Migration, as the CustomizationNetworkObject should only contain the new syntax
            var outfit = preset.CurrentOutfit.Clone();
            var data = new CustomizationNetworkObject
            {
                Name = preset.Name,
                Description = preset.Description,
                HeadPrefab = prefabs.Head.IndexOf(outfit.Head.Component),
                HeadTexture = outfit.Head.Texture,
                CurrentOutfit = outfit,
                CurrentBuild = preset.CurrentBuild
            };
            data.CurrentOutfit.Head = null;
            return data;
        }

        public CharacterPreset ToPreset(CharacterPrefabs prefabs)
        {
            var data = new CharacterPreset
            {
                Name = Name,
                Description = Description,
                CurrentOutfit = CurrentOutfit,
                CurrentBuild = CurrentBuild
            };
            data.CurrentOutfit.Head = new HumanHeadSelected
            {
                Component = prefabs.Head[HeadPrefab],
                Texture = HeadTexture
            };
            return data;
        }
    }
}
