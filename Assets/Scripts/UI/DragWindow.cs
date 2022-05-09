using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.UI
{
    public class DragWindow : MonoBehaviour, IDragHandler
    {
        [SerializeField] private RectTransform parent;
        [SerializeField] private Canvas canvas;

        public void OnDrag(PointerEventData eventData)
        {
            Vector2 mousePosition = new Vector2(UnityEngine.Input.mousePosition.x, UnityEngine.Input.mousePosition.y);

            mousePosition.x = Mathf.Clamp(mousePosition.x, 0 + parent.rect.width / 2, Screen.width - parent.rect.width / 2);
            mousePosition.y = Mathf.Clamp(mousePosition.y, 0 + parent.rect.height / 2, Screen.height - parent.rect.height / 2);

            parent.gameObject.transform.position = mousePosition;
        }
    }
}
