using System;
using UnityEngine;

namespace Assets.Scripts.Characters.Humans.Customization.Components
{
    [Serializable]
    public struct AdvancedOptions
    {
        [SerializeField] public EmblemComponent EmblemChest;
        [SerializeField] public EmblemComponent EmblemBack;
        [SerializeField] public EmblemComponent EmblemRightArm;
        [SerializeField] public EmblemComponent EmblemLeftArm;
    }
}
