using System;
using UnityEngine;

namespace Assets.Scripts.Characters.Humans.Customization.Components
{
    [CreateAssetMenu(fileName = "Human Hair", menuName = "Character/Hair")]
    public class HumanHairComponent : HumanComponent
    {
    }

    [Serializable]
    public class HumanHairSelected : HumanSelectedComponent<HumanHairComponent>
    {
        public Color Color;
    }
}
