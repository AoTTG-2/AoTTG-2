using JetBrains.Annotations;
using System;
using UnityEngine;

namespace Assets.Scripts.Characters.Humans.Customization.Components
{
    [Serializable]
    public struct FacialComponent
    {
        [SerializeField] public FacialTexture Texture;
        [SerializeField] [CanBeNull] public string CustomUrl;
        [SerializeField] public Color Color;
    }
    
    public enum FacialTexture
    {
        Custom,
        None,
        BeardFeng,
        BeardMike,
        Moustache1,
        Moustache2,
        Moustache3,
        Moustache4,
        Moustache5,
        Mouth1,
        Mouth2,
        Mouth3,
        Mouth4,
        Mouth5
    }
}
