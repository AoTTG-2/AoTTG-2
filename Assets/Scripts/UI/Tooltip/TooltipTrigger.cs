using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.UI.Tooltip
{
    public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private LTDescr delay;
        public string Header;
        public string Content;

        public void OnPointerEnter(PointerEventData eventData)
        {
            Debug.Log("OnPointerEnter");
            delay = LeanTween.delayedCall(0.5f, () =>
            {
                TooltipSystem.Show(Content, Header);
            });
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (LeanTween.isTweening())
            {
                LeanTween.cancel(delay.uniqueId);
            }
            TooltipSystem.Hide();
        }
    }
}
