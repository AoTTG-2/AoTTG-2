using JetBrains.Annotations;
using System;
using UnityEngine;

namespace Assets.Scripts.Characters.Humans.Customization.Components
{
    [Serializable]
    public struct EquipmentComponent
    {
        [SerializeField] public EquipmentTexture Texture;
        [SerializeField] [CanBeNull] public string CustomUrl;
        [SerializeField] public Color Color;
        [SerializeField] public HandGripComponent HandGrid;
    }
    
    public enum EquipmentTexture
    {
        Custom,
        Default,
        DefaultAHSS
    }
}
