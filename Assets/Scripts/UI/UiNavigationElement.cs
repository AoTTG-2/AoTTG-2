using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class UiNavigationElement : MonoBehaviour
    {
        public GameObject PreviousPage { get; set; }
        public UiHandler Canvas { get; set; }

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
    }
}
