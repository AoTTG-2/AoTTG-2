using Assets.Scripts.UI;
using Assets.Scripts.UI.InGame.HUD;

namespace Assets.Scripts.Services.Interface
{
    public interface IUiService
    {
        UiHandler GetUiHandler();
        void ResetMessage(LabelPosition label);
        void ResetMessagesAll();
        void SetMessage(LabelPosition label, string message);
    }
}
