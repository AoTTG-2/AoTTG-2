using Assets.Scripts.Characters.Humans.Customization.Components;
using Assets.Scripts.Characters.Humans.Equipment;
using Assets.Scripts.Characters.Humans.Skills;
using System;
using UnityEngine;

namespace Assets.Scripts.Characters.Humans.Customization
{
    [Serializable]
    //TODO: This may be made a struct again after equipment migration
    public class CharacterBuild
    {
        [SerializeField] public string Name;
        [SerializeField] public EquipmentType Equipment;
        [SerializeField] public EquipmentComponent EquipmentComponent;
        [SerializeField] public HeroStats Stats;
        [SerializeField] public HeroSkill Skill;
    }
}
