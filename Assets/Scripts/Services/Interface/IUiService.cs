using Assets.Scripts.UI;
using Assets.Scripts.UI.InGame.HUD;

namespace Assets.Scripts.Services.Interface
{
    public interface IUiService
    {
        /// <summary>
        /// Returns the UiHandler, which contains references to all static UI elements
        /// </summary>
        /// <returns></returns>
        UiHandler GetUiHandler();
        /// <summary>
        /// Resets the content of a HUD label located at <paramref name="label"/>
        /// </summary>
        /// <param name="label">The label which should have its content cleared</param>
        void ResetMessage(LabelPosition label);
        /// <summary>
        /// Resets the content of all labels in the HUD
        /// </summary>
        void ResetMessagesAll();
        /// <summary>
        /// Sets the content of a HUD label
        /// </summary>
        /// <param name="label">The position of the label</param>
        /// <param name="message">The localized string message</param>
        void SetMessage(LabelPosition label, string message);
    }
}
