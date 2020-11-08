using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Characters.Humans.Customization.Components
{
    [Serializable]
    public struct HairComponent
    {
        [SerializeField] public HairModel Model;
        [SerializeField] public HairTexture Texture;
        [SerializeField] [CanBeNull] public string CustomUrl;
        [SerializeField] public Color Color;
    }

    public enum HairModel
    {
        Annie,
        Armin,
        Male1,
        Male2,
        Male3,
        Male4,
        Eren,
        Female1,
        Female2,
        Female3,
        Female4,
        Female5,
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

    public enum HairTexture
    {
        Custom,
        Annie,
        Armin,
        Male1,
        Male2,
        Male3,
        Male4,
        Eren,
        Female1,
        Female2,
        Female3,
        Female4,
        Female5,
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
