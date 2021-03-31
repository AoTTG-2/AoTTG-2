using System;
using UnityEngine;

namespace Assets.Scripts.Characters.Humans.Customization.Components
{
    [CreateAssetMenu(fileName = "Human Eyes", menuName = "Character/Eyes")]
    public class HumanEyesComponent : HumanComponent
    {
    }

    [Serializable]
    public class HumanEyesSelected : HumanSelectedComponent<HumanEyesComponent>
    {

    }
}
