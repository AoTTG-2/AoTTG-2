using System;
using UnityEngine;

namespace Assets.Scripts.Characters.Humans.Customization.Components
{
    [CreateAssetMenu(fileName = "Human Facial", menuName = "Character/Facial")]
    public class FacialComponent : HumanComponent
    {
    }

    [Serializable]
    public class FacialSelected : HumanSelectedComponent<FacialComponent>
    {

    }
}
