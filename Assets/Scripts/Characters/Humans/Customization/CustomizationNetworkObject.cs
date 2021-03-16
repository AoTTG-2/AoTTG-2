using Assets.Scripts.Characters.Humans.Customization.Components;
using Assets.Scripts.Serialization;
using System;
using UnityEngine;

namespace Assets.Scripts.Characters.Humans.Customization
{
    public struct CustomizationNetworkObject
    {

        [SerializeField] public string Name;
        [SerializeField] public string Description;

        public CustomizationItem Head;
        public CustomizationItem Hair;
        public CustomizationItem Eyes;
        public CustomizationItem Glasses;
        public CustomizationItem Outfit;

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
                Head = new CustomizationItem(0, outfit.Head.Texture),
                Hair = new CustomizationItem(prefabs.Hair.IndexOf(outfit.Hair.Component), outfit.Hair.Texture),
                Eyes = new CustomizationItem(prefabs.Eyes.IndexOf(outfit.Eyes.Component), outfit.Eyes.Texture),
                Glasses = new CustomizationItem(prefabs.Glasses.IndexOf(outfit.Glasses.Component), outfit.Glasses.Texture),
                Outfit = new CustomizationItem(prefabs.Outfits.IndexOf(outfit.Outfit.Component), outfit.Outfit.Texture),
                CurrentOutfit = outfit,
                CurrentBuild = preset.CurrentBuild
            };
            data.CurrentOutfit.Head = null;
            data.CurrentOutfit.Hair = null;
            data.CurrentOutfit.Eyes = null;
            data.CurrentOutfit.Glasses = null;
            data.CurrentOutfit.Outfit = null;
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
                Component = prefabs.Head,
                Texture = Hair.PrefabTextureIndex
            };

            data.CurrentOutfit.Hair = new HumanHairSelected
            {
                Component = prefabs.Hair[Hair.PrefabIndex],
                Texture = Hair.PrefabTextureIndex
            };

            data.CurrentOutfit.Eyes = new HumanEyesSelected
            {
                Component = prefabs.Eyes[Eyes.PrefabIndex],
                Texture = Eyes.PrefabTextureIndex
            };

            data.CurrentOutfit.Glasses = new HumanGlassesSelected
            {
                Component = prefabs.Glasses[Glasses.PrefabIndex],
                Texture = Glasses.PrefabTextureIndex
            };

            data.CurrentOutfit.Outfit = new HumanOutfitSelected
            {
                Component = prefabs.Outfits[Outfit.PrefabIndex],
                Texture = Outfit.PrefabTextureIndex
            };
            return data;
        }
    }

    [Serializable]
    public struct CustomizationItem
    {
        public int PrefabIndex;
        public int PrefabTextureIndex;
        public SerializableColor Color;
        public string CustomTexture;

        public CustomizationItem(int prefabIndex, int prefabTextureIndex)
        {
            PrefabIndex = prefabIndex;
            PrefabTextureIndex = prefabTextureIndex;
            Color = new SerializableColor();
            CustomTexture = null;
        }
    }
}
