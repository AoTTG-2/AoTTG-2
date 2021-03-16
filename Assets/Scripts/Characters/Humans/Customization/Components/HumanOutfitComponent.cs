using System;
using UnityEngine;

namespace Assets.Scripts.Characters.Humans.Customization.Components
{
    [CreateAssetMenu(fileName = "Human Outfit", menuName = "Character/Outfit")]
    public class HumanOutfitComponent : HumanComponent
    {
        public Gender Gender;
        public GameObject ArmLeft;
        public GameObject ArmRight;
        public GameObject Legs;
    }

    [Serializable]
    public class HumanOutfitSelected : HumanSelectedComponent<HumanOutfitComponent>
    {

    }
}
