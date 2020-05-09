using System;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class UiHandler : MonoBehaviour
    {
        [HideInInspector]
        public UiNavigationElement[] Elements;

        public GameObject MenuUi;
        public GameObject InGameUi;

        public void Start()
        {
            ShowMenu();
            DontDestroyOnLoad(gameObject);
            Elements = gameObject.GetComponentsInChildren<UiNavigationElement>(true);
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

        public UiNavigationElement Find(Type t)
        {
            return Elements.SingleOrDefault(x => x.GetType() == t);
        }

    }
}
