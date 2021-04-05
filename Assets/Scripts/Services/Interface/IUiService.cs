using Assets.Scripts.UI.InGame.HUD;

namespace Assets.Scripts.Services.Interface
{
    public interface IUiService
    {
        void ResetMessage(LabelPosition label);
        void ResetMessagesAll();
        void SetMessage(LabelPosition label, string message);
    }
}
