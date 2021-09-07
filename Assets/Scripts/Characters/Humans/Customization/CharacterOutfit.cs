using Assets.Scripts.Characters.Humans.Customization.Components;
using System;
using UnityEngine;

namespace Assets.Scripts.Characters.Humans.Customization
{
    /// <summary>
    /// Contains of the human
    /// </summary>
    [Serializable]
    //TODO: This may be made a struct again after equipment migration
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
                Headgear = Headgear
            };
        }
    }
}
