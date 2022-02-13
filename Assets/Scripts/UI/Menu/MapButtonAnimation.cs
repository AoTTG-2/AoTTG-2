using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.UI.Menu
{
    class MapButtonAnimation : ButtonAnimation
    {
        [SerializeField]
        private RectTransform region;
        [SerializeField]
        private ServerSelector serverLogic;

        protected override void OnHover(PointerEventData eventData)
        {
            base.OnHover(eventData);
            serverLogic.MapMovement(region);
        }

        protected override void OnExit(PointerEventData eventData)
        {
            base.OnExit(eventData);
            serverLogic.MapMovement();
        }
    }
}
