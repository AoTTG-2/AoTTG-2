using Assets.Scripts.Characters.Humans.Customization.Components;
using Assets.Scripts.Extensions;
using Assets.Scripts.Serialization;
using System;
using UnityEngine;

namespace Assets.Scripts.Characters.Humans.Customization
{
    /// <summary>
    /// A struct which converts a <see cref="CharacterOutfit"/> to an object which can be sent over the network
    /// </summary>
    public struct CustomizationNetworkObject
    {

        [SerializeField] public string Name;
        [SerializeField] public string Description;

        public CustomizationItem Head;
        public CustomizationItem Hair;
        public CustomizationItem Eyes;
        public CustomizationItem Glasses;
        public CustomizationItem Facial;
        public CustomizationItem Outfit;
        public CustomizationItem Cape;
        public CustomizationItem Headgear;

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
                Hair = new CustomizationItem(prefabs.Hair.IndexOf(outfit.Hair.Component), outfit.Hair.Texture, outfit.Hair.Color),
                Eyes = new CustomizationItem(prefabs.Eyes.IndexOf(outfit.Eyes.Component), outfit.Eyes.Texture),
                Glasses = new CustomizationItem(prefabs.Glasses.IndexOf(outfit.Glasses.Component), outfit.Glasses.Texture),
                Facial = new CustomizationItem(prefabs.Facial.IndexOf(outfit.Facial.Component), outfit.Facial.Texture),
                Outfit = new CustomizationItem(prefabs.Outfits.IndexOf(outfit.Outfit.Component), outfit.Outfit.Texture),
                Cape = new CustomizationItem(prefabs.Cape.IndexOf(outfit.Cape.Component), outfit.Cape.Texture),
                Headgear = new CustomizationItem(prefabs.Headgear.IndexOf(outfit.Headgear.Component), outfit.Headgear.Texture),
                
                CurrentOutfit = outfit,
                CurrentBuild = preset.CurrentBuild
            };
            data.CurrentOutfit.Head = null;
            data.CurrentOutfit.Hair = null;
            data.CurrentOutfit.Eyes = null;
            data.CurrentOutfit.Glasses = null;
            data.CurrentOutfit.Facial = null;
            data.CurrentOutfit.Outfit = null;
            data.CurrentOutfit.Cape = null;
            data.CurrentOutfit.Headgear = null;
            return data;
        }

        /// <summary>
        /// Converts the CustomizationNetworkObject to a <see cref="CharacterPreset"/>
        /// </summary>
        /// <param name="prefabs">A reference to the <see cref="CharacterPrefabs"/></param>
        /// <returns></returns>
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
                Texture = Head.PrefabTextureIndex
            };

            data.CurrentOutfit.Hair = new HumanHairSelected
            {
                Component = prefabs.Hair.GetItemOrNull(Hair.PrefabIndex),
                Texture = Hair.PrefabTextureIndex,
                Color = Hair.Color.ToColor()
            };

            data.CurrentOutfit.Eyes = new HumanEyesSelected
            {
                Component = prefabs.Eyes.GetItemOrFirst(Eyes.PrefabIndex),
                Texture = Eyes.PrefabTextureIndex
            };

            data.CurrentOutfit.Headgear = new HeadgearSelected
            {
                Component = prefabs.Headgear.GetItemOrNull(Headgear.PrefabIndex),
                Texture = Headgear.PrefabTextureIndex
            };

            data.CurrentOutfit.Glasses = new HumanGlassesSelected
            {
                Component = prefabs.Glasses.GetItemOrNull(Glasses.PrefabIndex),
                Texture = Glasses.PrefabTextureIndex
            };

            data.CurrentOutfit.Facial = new FacialSelected
            {
                Component = prefabs.Facial.GetItemOrNull(Facial.PrefabIndex),
                Texture = Facial.PrefabTextureIndex
            };

            data.CurrentOutfit.Outfit = new HumanOutfitSelected
            {
                Component = prefabs.Outfits.GetItemOrFirst(Outfit.PrefabIndex),
                Texture = Outfit.PrefabTextureIndex
            };

            data.CurrentOutfit.Cape = new CapeSelected
            {
                Component = prefabs.Cape.GetItemOrNull(Cape.PrefabIndex),
                Texture = Outfit.PrefabTextureIndex
            };
            return data;
        }
    }

    [Serializable]
    public struct CustomizationItem
    {
        /// <summary>
        /// The index of the prefab within the <see cref="CharacterPrefabs"/>. This keeps the <see cref="CustomizationNetworkObject"/> lightweight
        /// </summary>
        public int PrefabIndex;
        /// <summary>
        /// The index of the texture within the <see cref="CharacterPrefabs"/> texture. This keeps the <see cref="CustomizationNetworkObject"/> lightweight
        /// </summary>
        public int PrefabTextureIndex;
        /// <summary>
        /// A color which can be sent over the network
        /// </summary>
        public SerializableColor Color;
        /// <summary>
        /// If this is not null, then the texture will be downloaded instead of using the <see cref="PrefabTextureIndex"/>
        /// </summary>
        public string CustomTexture;

        public CustomizationItem(int prefabIndex, int prefabTextureIndex)
        {
            PrefabIndex = prefabIndex;
            PrefabTextureIndex = prefabTextureIndex;
            Color = new SerializableColor();
            CustomTexture = null;
        }

        public CustomizationItem(int prefabIndex, int prefabTextureIndex, Color color)
        {
            PrefabIndex = prefabIndex;
            PrefabTextureIndex = prefabTextureIndex;
            Color = new SerializableColor(color);
            CustomTexture = null;
        }
    }
}
