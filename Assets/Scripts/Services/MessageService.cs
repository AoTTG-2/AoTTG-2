using Assets.Scripts.Room.Chat;
using Assets.Scripts.Services.Interface;
using Assets.Scripts.UI;
using UnityEngine;

namespace Assets.Scripts.Services
{
    public class MessageService : MonoBehaviour, IMessageService
    {
        [SerializeField] private InRoomChat chat;

        public void Local(string message, DebugLevel level)
        {
            chat.AddMessage($"<color=#{GetColorCode(level)}>{level.ToString().ToUpperInvariant()}</color>: {message}");
        }

        private string GetColorCode(DebugLevel level)
        {
            if (level == DebugLevel.Critical)
            {
                return "FF0000";
            }

            return "FFFFFF";
        }
    }
}
