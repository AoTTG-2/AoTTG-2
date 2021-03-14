using JetBrains.Annotations;
using System;
using UnityEngine;

namespace Assets.Scripts.Characters.Humans.Customization.Components
{
    [Serializable]
    public struct HandGripComponent
    {
        [SerializeField] public HandGripTexture Texture;
        [SerializeField] [CanBeNull] public string CustomUrl;
        [SerializeField] public Color Color;
    }

    public enum HandGripTexture
    {
        Custom,
        Default,
        DefaultAHSS
    }
}
