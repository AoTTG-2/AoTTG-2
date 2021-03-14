using JetBrains.Annotations;
using System;
using UnityEngine;

namespace Assets.Scripts.Characters.Humans.Customization.Components
{
    [Serializable]
    public struct EmblemComponent
    {
        [SerializeField] public EmblemTexture Texture;
        [SerializeField] [CanBeNull] public string CustomUrl;
        [SerializeField] public Color Color;
    }

    public enum EmblemTexture
    {
        Custom,
        None,
        SurveyCorps,
        Garrison,
        MilitaryPolice,
        TrainingCorps
    }
}
