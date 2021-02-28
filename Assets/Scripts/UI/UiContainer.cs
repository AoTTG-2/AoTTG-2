using Assets.Scripts.UI.Menu;
using System.Collections.Generic;

namespace Assets.Scripts.UI
{
    /// <summary>
    /// UIContainers contain IUIElements as children.
    /// </summary>
    public abstract class UiContainer : UiElement, IUiContainer
    {
        protected List<IUiElement> children = new List<IUiElement>();

        public int GetNumVisibleChildren() => children.FindAll(e => e.IsVisible()).Count;

        public void AddChild(IUiElement element) => children.Add(element);

        public void RemoveChild(IUiElement element) => children.Remove(element);

        public List<IUiElement> GetChildren() => children;
    }
}
