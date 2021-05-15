using Assets.Scripts.UI;

namespace Assets.Scripts.Services.Interface
{
    public interface IMessageService
    {
        void Local(string message, DebugLevel level);
    }
}
