using System;
using UnityEngine;

namespace Assets.Scripts.Characters.Humans.Customization.Components
{
    [CreateAssetMenu(fileName = "Human Outfit", menuName = "Character/Outfit")]
    public class HumanOutfitComponent : HumanComponent
    {
        public Gender Gender;
        public ArmComponent ArmLeft;
        public ArmComponent ArmRight;
        public GameObject Legs;

        public GameObject EmblemFront;
        public GameObject EmblemBack;
    }

    [Serializable]
    public class HumanOutfitSelected : HumanSelectedComponent<HumanOutfitComponent>
    {
        public bool UseCapeEmblem;
        public EmblemSelected EmblemFront;
        public EmblemSelected EmblemBack;
        public EmblemSelected EmblemRight;
        public EmblemSelected EmblemLeft;
    }
}
