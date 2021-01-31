using System;
using Photon;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class UiNavigationElement : PunBehaviour
    {
        public UiHandler Canvas { get; set; }

        public GameObject PreviousPage { get; set; }

        public virtual void Back()
        {
            gameObject.SetActive(false);
            PreviousPage.SetActive(true);
        }

        public void Navigate(Type type)
        {
            var target = Canvas.Find(type);
            if (target == null)
            {
                Debug.LogError("Could not find target");
                return;
            }

            target.gameObject.SetActive(true);
            target.PreviousPage = gameObject;
            gameObject.SetActive(false);
        }

        protected virtual void OnDisable()
        {
            MenuManager.RegisterClosed();
        }

        protected virtual void OnEnable()
        {
            MenuManager.RegisterOpened();
        }
    }
}