using System;
using UnityEngine;

namespace Assets.Scripts.Characters.Humans.Customization.Components
{
    [CreateAssetMenu(fileName = "Human Emblem", menuName = "Character/Emblem")]
    public class EmblemComponent : HumanComponent
    {
    }

    [Serializable]
    public class EmblemSelected : HumanSelectedComponent<EmblemComponent>
    {

    }
}
