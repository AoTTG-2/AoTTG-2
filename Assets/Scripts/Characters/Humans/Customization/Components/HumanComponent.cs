using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Characters.Humans.Customization.Components
{
    //TODO: This one should be made abstract, and then each character component has a strongly typed version
    [CreateAssetMenu(fileName = "New Human Component", menuName = "Character/Character Component")]
    public class HumanComponent : ScriptableObject
    {
        public GameObject Model;
        public List<Texture2D> Textures;
    }

    [Serializable]
    public class HumanSelectedComponent
    {
        public HumanComponent Component;
        public int Texture;
    }
}
