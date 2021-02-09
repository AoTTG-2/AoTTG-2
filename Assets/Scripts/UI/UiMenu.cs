namespace Assets.Scripts.UI.InGame
{
    /// <summary>
    /// A UI Menu will be registered by the Menu Manager.
    /// </summary>
    public abstract class UiMenu : UiContainer
    {
        protected virtual void OnEnable()
        {
            MenuManager.RegisterOpened(this);
        }

        protected virtual void OnDisable()
        {
            MenuManager.RegisterClosed(this);
        }
    }
}
