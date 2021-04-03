using Assets.Scripts.UI.Menu;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public abstract class UiElement : MonoBehaviour, IUiElement
    {
        public virtual bool IsVisible() => gameObject.activeSelf;

        public virtual void Show() => gameObject.SetActive(true);

        public virtual void Hide() => gameObject.SetActive(false);
    }
}
