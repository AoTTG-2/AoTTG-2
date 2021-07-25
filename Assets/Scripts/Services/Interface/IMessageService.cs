using Assets.Scripts.UI;

namespace Assets.Scripts.Services.Interface
{
    /// <summary>
    /// A Service for notifications and chat messages
    /// </summary>
    public interface IMessageService
    {
        /// <summary>
        /// Writes a message to the chatbox. Will not be sent to other players.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="level"></param>
        void Local(string message, DebugLevel level);
    }
}
