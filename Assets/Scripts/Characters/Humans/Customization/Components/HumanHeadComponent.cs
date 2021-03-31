using System;
using UnityEngine;

namespace Assets.Scripts.Characters.Humans.Customization.Components
{
    [CreateAssetMenu(fileName = "Human Head", menuName = "Character/Head")]
    public class HumanHeadComponent : HumanComponent
    {
    }

    [Serializable]
    public class HumanHeadSelected : HumanSelectedComponent<HumanHeadComponent>
    {

    }
}
