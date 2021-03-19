using Assets.Scripts.Characters.Humans.Customization.Components;
using System;
using UnityEngine;

namespace Assets.Scripts.Characters.Humans.Customization
{
    [Serializable]
    //TODO: 599 This can be made a struct again after migration
    public class CharacterOutfit
    {
        [SerializeField] public string Name;
        [SerializeField] public Gender Gender;
        [SerializeField] public HumanHeadSelected Head;
        [SerializeField] public SkinComponent Skin;
        [SerializeField] public HumanHairSelected Hair;
        [SerializeField] public HumanEyesSelected Eyes;
        [SerializeField] public HumanGlassesSelected Glasses;
        [SerializeField] public FacialSelected Facial;
        [SerializeField] public HumanOutfitSelected Outfit;
        [SerializeField] public CapeSelected Cape;
        [SerializeField] public AdvancedOptions Advanced;

        [SerializeField] public HeadgearSelected Headgear;

        public CharacterOutfit Clone()
        {
            return new CharacterOutfit
            {
                Name = Name,
                Gender = Gender,
                Head = Head,
                Skin = Skin,
                Hair = Hair,
                Eyes = Eyes,
                Glasses = Glasses,
                Facial = Facial,
                Outfit = Outfit,
                Cape = Cape,
                Advanced = Advanced,
                Headgear = Headgear
            };
        }
    }
}
