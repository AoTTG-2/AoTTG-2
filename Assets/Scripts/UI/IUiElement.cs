namespace Assets.Scripts.UI.Menu
{
    public interface IUiElement
    {
        /// <summary>
        /// Whether the current UI menu is visible or not.
        /// </summary>
        bool IsVisible();

        /// <summary>
        /// Makes the element visible.
        /// </summary>
        void Show();

        /// <summary>
        /// Hides the element from view.
        /// </summary>
        void Hide();
    }
}
