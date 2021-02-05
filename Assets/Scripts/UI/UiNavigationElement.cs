using System;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class UiNavigationElement : UiMenuPUN
    {
        public UiHandler Canvas { get; set; }

        public GameObject PreviousPage { get; set; }

        public virtual void Back()
        {
            Hide();
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

            target.Show();
            target.PreviousPage = gameObject;
            Hide();
        }
    }
}