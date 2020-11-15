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
        Eye1,
        Eye2,
        Eye3,
        Eye4,
        Eye5,
        Eye6,
        Eye7,
        Eye8,
        Eye9,
        Eye10,
        Eye11,
        Eye12,
        Eye13,
        Eye14,
        Annie,
        Armin,
        Connie,
        Eren,
        Hanji,
        Jean,
        Levi,
        Marco,
        Mikasa,
        Mike,
        Petra,
        Rico,
        Sasha
    }
}
