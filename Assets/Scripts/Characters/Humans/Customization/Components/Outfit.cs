using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Characters.Humans.Customization.Components
{
    [Serializable]
    public struct OutfitComponent
    {
        [SerializeField] public OutfitModel Model;
        [SerializeField] public OutfitTexture Texture;
        [SerializeField] [CanBeNull] public string CustomUrl;
        [SerializeField] public Color Color;
    }

    public enum OutfitModel
    {
        CasualFemaleA,
        CasualFemaleB,
        CasualMaleA,
        CasualMaleB,
        UniformFemaleA,
        UniformFemaleB,
        UniformMaleA,
        UniformMaleB
    }

    public enum OutfitTexture
    {
        Custom,
        Levi,
        CasualFemaleA,
        CasualFemaleB,
        CasualMaleA,
        CasualMaleB,
        UniformFemaleA,
        UniformFemaleB,
        UniformMaleA,
        UniformMaleB
    }
}
