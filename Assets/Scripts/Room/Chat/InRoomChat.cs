using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Event;
using static FengGameManagerMKII;
using System.Linq;
using Assets.Scripts.Room.Chat;
using System.Web;
using System.Net;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;

public class InRoomChat : Photon.MonoBehaviour
{
    private const int MaxStoredMessages = 100;
    private const int MaxMessageLength = 1000;
    private bool alignBottom = true;
    public static readonly string ChatRPC = "Chat";
    private string inputLine = string.Empty;
    public bool IsVisible = true;
    private readonly List<string> messages = new List<string>();
    private Vector2 scrollPos = Vector2.zero;
    public InputField ChatInputField;
    public Text ChatText;
    
    /// <summary>
    /// Returns list of messages in chatroom
    /// </summary>
    /// <returns></returns>
    public List<string> GetMessages()
    {
        return messages;
    }
    /// <summary>
    /// Clears Message List
    /// </summary>
    public void ClearMessages()
    {
        messages.Clear();
    }

    /// <summary>
    /// Adds message to local message list
    /// </summary>
    /// <param name="message"></param>
    public void AddMessage(string message)
    {
        if (message.Count() <= MaxMessageLength)
        {
            RemoveMessageIfMoreThenMax();
            messages.Add(message);
        }
        else
        {
            Assets.Scripts.Room.Chat.ChatCommandHandler.OutputErrorMessage($"Message can not have more than {MaxMessageLength} characters");
        }
    }

    /// <summary>
    /// Removes messages when exceding max storage
    /// </summary>
    private void RemoveMessageIfMoreThenMax()
    {
        if (messages.Count() > MaxStoredMessages)
        {
            messages.RemoveAt(0);
        }
    }

    /// <summary>
    /// Send message to all clients on the server
    /// </summary>
    /// <param name="message"></param>
    private void ChatAll(string message)
    {
        if (message.Count() <= 1000)
        {
            if (CheckMarkup(message))
            {
                var chatMessage = new object[] { message, Assets.Scripts.Room.Chat.ChatCommandHandler.SetNameColorDependingOnteam(PhotonNetwork.player) };
                instance.photonView.RPC("Chat", PhotonTargets.All, chatMessage);
            }
            else
            {
                Assets.Scripts.Room.Chat.ChatCommandHandler.OutputErrorMessage("Bad markup.");
            }
            
        }
        else
        {
            Assets.Scripts.Room.Chat.ChatCommandHandler.OutputErrorMessage($"Message can not have more than {MaxMessageLength} characters");
        }
    }

    public void OnGUI()
    {
        if (!IsVisible || (PhotonNetwork.connectionState != ConnectionState.Connected))
        {
            return;
        }
        if (current.type == EventType.KeyDown)
        {
            if ((((current.keyCode != KeyCode.Tab) && (current.character != '\t')) || IN_GAME_MAIN_CAMERA.isPausing) || (inputRC.humanKeys[InputCodeRC.chat] == KeyCode.Tab))
            {
                goto Label_00E1;
            }
            current.Use();
            goto Label_013D;
        }
        if ((current.type == EventType.KeyUp) && (((current.keyCode != KeyCode.None) && (current.keyCode == inputRC.humanKeys[InputCodeRC.chat])) && (GUI.GetNameOfFocusedControl() != "ChatInput")))
        {
            inputLine = string.Empty;
            ChatInputField.gameObject.GetComponent<Text>().text = string.Empty;
            goto Label_013D;
        }
    Label_00E1:
        if ((current.type == EventType.KeyDown) && ((current.keyCode == KeyCode.KeypadEnter) || (current.keyCode == KeyCode.Return)))
        {
            if (!string.IsNullOrEmpty(inputLine))
            {
                if (inputLine == "\t")
                {
                    inputLine = string.Empty;
                    ChatInputField.gameObject.GetComponent<Text>().text = string.Empty;
                    return;
                }
                if (RCEvents.ContainsKey("OnChatInput"))
                {
                    var key = (string) RCVariableNames["OnChatInput"];
                    if (stringVariables.ContainsKey(key))
                    {
                        stringVariables[key] = inputLine;
                    }
                    else
                    {
                        stringVariables.Add(key, inputLine);
                    }
                }

                if (!inputLine.StartsWith("/"))
                {
                    ChatAll(inputLine);
                }
                else
                {
                    ChatCommandHandler(inputLine);
                }
                
                inputLine = string.Empty;
                ChatInputField?.Select();
                ChatInputField.text = string.Empty;

                return;
            }
            inputLine = "\t";
            ChatInputField?.Select();
        }
    Label_013D:
        var text = string.Empty;
        for (var i = 0; i < messages.Count; i++)
        {
            text += messages[i] + "\n";
        }

        if (ChatText != null)
        {
            ChatText.text = text;
        }

        inputLine = ChatInputField?.text;
    }

    /// <summary>
    /// Hanlde commands in chat
    /// </summary>
    /// <param name="input"></param>
    private void ChatCommandHandler(string input)
    {
        Assets.Scripts.Room.Chat.ChatCommandHandler.CommandHandler(input);
    }

    /// <summary>
    /// Check if message contains valid markup
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    private bool CheckMarkup(string message)
    {
        return Regex.Matches(message, "[<,>]").Count % 2 == 0;
    }
}