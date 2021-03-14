using JetBrains.Annotations;
using System;
using UnityEngine;

namespace Assets.Scripts.Characters.Humans.Customization.Components
{
    [Serializable]
    public struct GlassesComponent
    {
        [SerializeField] public GlassesTexture Texture;
        [SerializeField] [CanBeNull] public string CustomUrl;
        [SerializeField] public Color Color;
    }

    public enum GlassesTexture
    {
        Custom,
        None,
        Glasses1,
        Glasses2,
        Glasses3,
        Glasses4,
        Glasses5,
        Glasses6,
        Feng,
        Hanji,
        Rico
    }
}
