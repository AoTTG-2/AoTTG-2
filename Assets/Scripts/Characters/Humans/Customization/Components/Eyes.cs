using JetBrains.Annotations;
using System;
using UnityEngine;

namespace Assets.Scripts.Characters.Humans.Customization.Components
{
    [Serializable]
    public struct EyesComponent
    {
        [SerializeField] public EyesModel Model;
        [SerializeField] public EyesTexture Texture;
        [SerializeField] [CanBeNull] public string CustomUrl;
        [SerializeField] public Color Color;
    }

    public enum EyesModel
    {
        Default
    }

    public enum EyesTexture
    {
        Custom,
        Annie,
        Levi
    }
}
