using Assets.Scripts.UI.Menu;
using System.Collections.Generic;

namespace Assets.Scripts.UI
{
    public interface IUiContainer : IUiElement
    {
        /// <summary>
        /// Returns a list of all the UI Menu that are under this UI menu.
        /// </summary>
        /// <returns></returns>
        List<IUiElement> GetChildren();

        /// <summary>
        /// Number of child UI elemnets that are visible.
        /// </summary>
        /// <returns></returns>
        int GetNumVisibleChildren();

        /// <summary>
        /// Adds the UI element under this container.
        /// </summary>
        /// <param name="element"></param>
        void AddChild(IUiElement element);

        /// <summary>
        /// Adds the UI element under this container.
        /// </summary>
        /// <param name="element"></param>
        void RemoveChild(IUiElement element);
    }
}
