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
        [SerializeField] public HairComponent Hair;
        [SerializeField] public EyesComponent Eyes;
        [SerializeField] public GlassesComponent Glasses;
        [SerializeField] public FacialComponent Facial;
        [SerializeField] public OutfitComponent Outfit;
        [SerializeField] public CapeComponent Cape;
        [SerializeField] public AdvancedOptions Advanced;

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
                Advanced = Advanced
            };
        }
    }
}
