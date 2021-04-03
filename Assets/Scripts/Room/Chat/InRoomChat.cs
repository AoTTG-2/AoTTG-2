using Assets.Scripts.UI.Input;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Assets.Scripts.FengGameManagerMKII;
using static Assets.Scripts.Room.Chat.ChatUtility;

public class InRoomChat : Photon.MonoBehaviour
{
    private const int MaxStoredMessages = 100;
    private const int MaxMessageLength = 1000;
    public static readonly string ChatRPC = "Chat";
    private string inputLine = string.Empty;
    public bool IsVisible = true;
    private readonly List<string> messages = new List<string>();
    public InputField ChatInputField;
    public Text ChatText;
    private bool IsChatOpen { get; set; }

    private void Update()
    {
        if (!IsVisible || (PhotonNetwork.connectionState != ConnectionState.Connected))
        {
            return;
        }
        
        HandleChatInput(this);

        UpdateChat(this);
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
            if (MenuManager.IsMenuOpen && IsChatOpen)
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
                MenuManager.RegisterClosed();
            }
            else if (!MenuManager.IsMenuOpen)
            {
                chat.ChatInputField?.Select();
                MenuManager.RegisterOpened();
                IsChatOpen = true;
            }
        }
    }

    private void UpdateChat(InRoomChat chat)
    {
        StringBuilder messageHandler = new StringBuilder();
        foreach (string message in messages)
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
        var countClosingTags =  Regex.Matches(message, @"<\W{1}\w+>").Count;

        return countOpeningTags == countClosingTags;
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
}
