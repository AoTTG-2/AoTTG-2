using System;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class UiHandler : MonoBehaviour
    {
        [HideInInspector]
        public UiElement[] Elements;

        public GameObject MenuUi;
        public GameObject InGameUi;

        public void Start()
        {
            ShowMenu();
            DontDestroyOnLoad(gameObject);
            Elements = gameObject.GetComponentsInChildren<UiElement>(true);
            foreach (var element in Elements)
            {
                element.Canvas = this;
                element.gameObject.SetActive(false);
            }
            Elements[0].gameObject.SetActive(true);
        }

        public void ShowMenu()
        {
            MenuUi.SetActive(true);
            InGameUi.SetActive(false);
        }

        public void ShowInGameUi()
        {
            InGameUi.SetActive(true);
            MenuUi.SetActive(false);
        }

        public UiElement Find(Type t)
        {
            return Elements.SingleOrDefault(x => x.GetType() == t);
        }

    }
}
