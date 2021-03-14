using System;
using UnityEngine;

namespace Assets.Scripts.Characters.Humans.Customization.Components
{
    [Serializable]
    public struct HeadComponent
    {
        [SerializeField] public HeadModel Model;
    }

    public enum HeadModel
    {
        Default
    }
}
