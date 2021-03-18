using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MapEditor
{
    public class NoDragScrollView : ScrollRect
    {
        public override void OnBeginDrag(PointerEventData eventData)
        {
            eventData.pointerDrag = null;
        }
        public override void OnDrag(PointerEventData eventData)
        {
            eventData.pointerDrag = null;
        }
        public override void OnEndDrag(PointerEventData eventData)
        {
            eventData.pointerDrag = null;
        }
    }
}