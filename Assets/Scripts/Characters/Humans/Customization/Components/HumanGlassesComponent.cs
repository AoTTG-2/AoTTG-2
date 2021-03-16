using System;
using UnityEngine;

namespace Assets.Scripts.Characters.Humans.Customization.Components
{
    [CreateAssetMenu(fileName = "Human Glasses", menuName = "Character/Glasses")]
    public class HumanGlassesComponent : HumanComponent
    {
    }

    [Serializable]
    public class HumanGlassesSelected : HumanSelectedComponent<HumanGlassesComponent>
    {

    }
}
