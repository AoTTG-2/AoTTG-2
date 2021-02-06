using Assets.Scripts.UI.Input;
using Assets.Scripts.UI.Menu;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine.EventSystems;
//using UnityEngine.UI;
using TMPro;
using static Assets.Scripts.FengGameManagerMKII;
using static Assets.Scripts.Room.Chat.ChatUtility;

namespace Assets.Scripts.Room.Chat
{
    public class InRoomChat : Photon.MonoBehaviour, IUiElement
    {
        private const int MaxStoredMessages = 100;
        private const int MaxMessageLength = 1000;
        public static readonly string ChatRPC = "Chat";
        private string inputLine = string.Empty;
        private readonly List<string> messages = new List<string>();
        public TMP_InputField ChatInputField;
        public TMP_Text ChatText;
        private bool IsChatOpen { get; set; }

        public bool IsVisible()
        {
            return gameObject.activeSelf;
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void ClearMessages()
        {
            messages.Clear();
        }

        public void AddMessage(string message)
        {
            TrimMessage(message);
            if (message.Count() > MaxMessageLength)
            {
                OutputErrorMessage($"Message can not have more than {MaxMessageLength} characters");
                return;
            }
            RemoveMessageIfMoreThanMax();
            messages.Add(message);
        }
        public void OutputSystemMessage(string input)
        {
            var message = $"<color=#FFCC00>{input}</color>"; ;
            instance.chatRoom.AddMessage(message);
        }

        /// <summary>
        /// Formats text as <color=#FF0000>Error: {input}</color> and outputs to chat
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public void OutputErrorMessage(string input)
        {
            var message = $"<color=#FF0000>Error: {input}</color>";
            instance.chatRoom.AddMessage(message);
        }

        /// <summary>
        /// Outputs Not Master Client Error to chat
        /// </summary>
        public void OutputErrorNotMasterClient()
        {
            OutputErrorMessage("Not Master Client");
        }

        /// <summary>
        /// Outputs Flayer Not Found Error to chat
        /// </summary>
        /// <param name="playerId"></param>
        public void OutputErrorPlayerNotFound(string playerId)
        {
            OutputErrorMessage($"No player with ID #{playerId} could be found.");
        }

        private void Update()
        {
            if (!IsVisible() || PhotonNetwork.connectionState != ConnectionState.Connected)
            {
                return;
            }

            HandleChatInput(this);

            UpdateChat(this);
        }

        private void TrimMessage(string message)
        {
            message.Trim();
        }

        private void RemoveMessageIfMoreThanMax()
        {
            if (messages.Count() == MaxStoredMessages)
            {
                messages.RemoveAt(0);
            }
        }

        private void ChatAll(string message)
        {
            if (message.Count() > MaxMessageLength)
            {
                OutputErrorMessage($"Message can not have more than {MaxMessageLength} characters");
                return;
            }
            if (MarkupIsOk(message))
            {
                var chatMessage = new object[] { message, GetPlayerName(PhotonNetwork.player) };
                instance.photonView.RPC(ChatRPC, PhotonTargets.All, chatMessage);
            }
            else
            {
                OutputErrorMessage("Bad markup. Make sure your tags are balanced.");
            }
        }

        private void HandleChatInput(InRoomChat chat)
        {
            if (InputManager.KeyDown(InputUi.Chat))
            {
                if (MenuManager.IsAnyMenuOpen && IsChatOpen)
                {
                    if (!string.IsNullOrEmpty(inputLine))
                    {
                        if (inputLine.StartsWith("/"))
                        {
                            CommandHandler(chat.inputLine);
                        }
                        else
                        {
                            ChatAll(chat.inputLine);
                        }
                    }
                    chat.inputLine = string.Empty;
                    chat.ChatInputField.text = string.Empty;
                    EventSystem.current.SetSelectedGameObject(null);
                    IsChatOpen = false;
                    MenuManager.RegisterClosed(this);
                }
                else if (!MenuManager.IsAnyMenuOpen)
                {
                    chat.ChatInputField?.Select();
                    MenuManager.RegisterOpened(this);
                    IsChatOpen = true;
                }
            }
        }

        private void UpdateChat(InRoomChat chat)
        {
            var messageHandler = new StringBuilder();
            foreach (var message in messages)
            {
                messageHandler.AppendLine(message);
            }

            if (ChatText != null)
            {
                chat.ChatText.text = messageHandler.ToString();
            }

            chat.inputLine = chat.ChatInputField?.text;
        }

        private void CommandHandler(string input)
        {
            ChatCommandHandler.CommandHandler(input);
        }

        private bool MarkupIsOk(string message)
        {
            var countOpeningTags = Regex.Matches(message, @"<\w+.{0,2}\w+>").Count;
            var countClosingTags = Regex.Matches(message, @"<\W{1}\w+>").Count;

            return countOpeningTags == countClosingTags;
        }

    }
}