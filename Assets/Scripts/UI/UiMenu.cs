using UnityEngine;

namespace Assets.Scripts.UI.InGame
{
    /// <summary>
    /// A UI Menu will be registered by the Menu Manager.
    /// </summary>
    public abstract class UiMenu : UiContainer
    {
        protected virtual void OnEnable()
        {
            Debug.Log($"{this.name} enabled.");
            MenuManager.RegisterOpened(this);
        }

        protected virtual void OnDisable()
        {
            Debug.Log($"{this.name} disabled.");
            MenuManager.RegisterClosed(this);
        }
    }
}
