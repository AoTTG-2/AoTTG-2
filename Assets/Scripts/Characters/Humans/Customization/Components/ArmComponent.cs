using System;
using UnityEngine;

namespace Assets.Scripts.Characters.Humans.Customization.Components
{
    [CreateAssetMenu(fileName = "Human Arm", menuName = "Character/Arm")]
    public class ArmComponent : HumanComponent
    {
        public GameObject Emblem;
    }

    [Serializable]
    public class ArmSelected : HumanSelectedComponent<ArmComponent>
    {

    }
}
