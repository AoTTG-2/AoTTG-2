using System;
using UnityEngine;

namespace Assets.Scripts.Characters.Humans.Customization.Components
{
    [CreateAssetMenu(fileName = "Human Cape", menuName = "Character/Cape")]
    public class CapeComponent : HumanComponent
    {
    }

    [Serializable]
    public class CapeSelected : HumanSelectedComponent<CapeComponent>
    {

    }
}
