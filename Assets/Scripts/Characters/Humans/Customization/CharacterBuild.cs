using System;
using Assets.Scripts.Characters.Humans.Customization.Components;
using UnityEngine;

namespace Assets.Scripts.Characters.Humans.Customization
{
    [Serializable]
    public struct CharacterBuild
    {
        [SerializeField] public EquipmentType Equipment;
        [SerializeField] public EquipmentComponent EquipmentComponent;
    }
}
