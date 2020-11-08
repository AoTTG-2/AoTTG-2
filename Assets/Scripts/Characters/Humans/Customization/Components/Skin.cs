using JetBrains.Annotations;
using System;
using UnityEngine;

namespace Assets.Scripts.Characters.Humans.Customization.Components
{
    [Serializable]
    public struct SkinComponent
    {
        [SerializeField] public Skin Skin;
        [SerializeField] [CanBeNull] public string CustomUrl;
        [SerializeField] public Color Color;
    }

    public enum Skin
    {
        SkinLight,
        SkinDark
    }
}
