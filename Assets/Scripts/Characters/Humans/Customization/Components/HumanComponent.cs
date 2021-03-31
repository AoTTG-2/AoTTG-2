using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Characters.Humans.Customization.Components
{
    public abstract class HumanComponent : ScriptableObject
    {
        public GameObject Model;
        public List<Texture2D> Textures;
    }

    [Serializable]
    public abstract class HumanSelectedComponent<T> where T : HumanComponent
    {
        public T Component;
        public int Texture;
    }
}
