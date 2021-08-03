using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.UI.Tooltip
{
    public class TooltipSystem : MonoBehaviour
    {
        private static TooltipSystem self;
        public Tooltip Tooltip;

        private void Awake()
        {
            if (self != null)
            {
                Debug.LogWarning("A tooltip system is already defined!");
                return;
            }
            self = this;
        }

        public static void Show(string content, string header = "")
        {
            self.Tooltip.SetText(content, header);
            self.Tooltip.gameObject.SetActive(true);
        }

        public static void Hide()
        {
            self.Tooltip.gameObject.SetActive(false);
        }
    }
}
