using System;
using UnityEngine;

namespace Assets.Scripts.Characters.Humans.Customization.Components
{
    [CreateAssetMenu(fileName = "Human Headgear", menuName = "Character/Headgear")]
    public class HeadgearComponent : HumanComponent
    {
    }

    [Serializable]
    public class HeadgearSelected : HumanSelectedComponent<HeadgearComponent>
    {

    }
}
