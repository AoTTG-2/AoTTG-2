﻿using JetBrains.Annotations;
using System;
using UnityEngine;

namespace Assets.Scripts.Characters.Humans.Customization.Components
{
    [Serializable]
    public struct CapeComponent
    {
        [SerializeField] public CapeTexture Texture;
        [SerializeField] [CanBeNull] public string CustomUrl;
        [SerializeField] public Color Color;
    }

    public enum CapeTexture
    {
        Custom,
        None,
        SurveyCorps,
        Garrison,
        MilitaryPolice,
        TrainingCorps
    }
}