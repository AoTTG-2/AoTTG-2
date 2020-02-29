using System;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class UiHandler : MonoBehaviour
    {
        [HideInInspector]
        public UiElement[] Elements;

        public void Start()
        {
            Elements = gameObject.GetComponentsInChildren<UiElement>(true);
            foreach (var element in Elements)
            {
                element.Canvas = this;
                element.gameObject.SetActive(false);
            }
            Elements[0].gameObject.SetActive(true);
        }

        public UiElement Find(Type t)
        {
            return Elements.SingleOrDefault(x => x.GetType() == t);
        }

    }
}
