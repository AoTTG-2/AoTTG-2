using Assets.Scripts.Characters.Humans.Customization.Components;
using System;
using UnityEngine;

namespace Assets.Scripts.Characters.Humans.Customization
{
    [Serializable]
    public struct CharacterOutfit
    {
        [SerializeField] public string Name;
        [SerializeField] public Gender Gender;
        [SerializeField] public HeadComponent Head;
        [SerializeField] public SkinComponent Skin;
        [SerializeField] public HairComponent Hair;
        [SerializeField] public EyesComponent Eyes;
        [SerializeField] public GlassesComponent Glasses;
        [SerializeField] public FacialComponent Facial;
        [SerializeField] public OutfitComponent Outfit;
        [SerializeField] public CapeComponent Cape;
        [SerializeField] public AdvancedOptions Advanced;
    }
}
