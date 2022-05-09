using Assets.Scripts.UI.Menu;
using UnityEngine;

namespace Assets.Scripts.UI
{
    /// <inheritdoc cref="IUiElement"/>
    /// <summary>
    /// The abstract Ui Element, which provides various utility methods
    /// </summary>
    public abstract class UiElement : MonoBehaviour, IUiElement
    {
        public virtual bool IsVisible() => gameObject.activeSelf;

        public virtual void Show() => gameObject.SetActive(true);

        public virtual void Hide() => gameObject.SetActive(false);
    }
}
