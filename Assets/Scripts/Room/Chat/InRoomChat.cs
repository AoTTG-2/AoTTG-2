using Assets.Scripts.Characters.Titan;
using Assets.Scripts.Gamemode.Options;
using ExitGames.Client.Photon;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Event;
using static PhotonNetwork;
using static FengGameManagerMKII;
using static Assets.Scripts.Room.Chat.CommandLineHandler;
using WebSocketSharp;
using System.Linq;

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
    /// Adds message to message list
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
            message = FormatErrorMessage($"Message can not have more than {MaxMessageLength} characters");
            AddMessage(message);
        }
    }

    private void RemoveMessageIfMoreThenMax()
    {
        if (messages.Count() >= MaxStoredMessages)
        {
            messages.RemoveAt(0);
        }
    }

    
    private void ChatAll(string message)
    {
        if (message.Count() <= 1000)
        {
            var chatMessage = new object[] { message, SetNameColorDependingOnteam(player) };
            instance.photonView.RPC("Chat", PhotonTargets.All, chatMessage);
        }
        else
        {
            message = FormatErrorMessage($"Message can not have more than {MaxMessageLength} characters");
            AddMessage(message);
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
                    ChatCommandLineHandler(inputLine);
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

private void ChatCommandLineHandler(string input)
    {
        var subStrings = input.Substring(1).Split(' ');
        string message;
        switch (subStrings[0])
        {
            // Info: 
            case "cloth":
                message = ClothFactory.GetDebugInfo();
                AddMessage(message);
                break;
            // Info: 
            case "aso":
                PreserveKdrOnOFF(subStrings);
                break;
            // Info: Pauses the game
            case "pause":
                PauseGame();
                break;
            // Info: Unpauses the game
            case "unpause":
                UnPauseGame();
                break;
            // Info: Outputs name of level
            case "checklevel":
                CheckLevel(player);
                break;
            // Info: 
            case "isrc":
                OutputIsRc();
                break;
            // Info: 
            case "ignorelist":
                OutputIgnoreList();
                break;
            // Info: 
            case "room":
                ChangeRoomProperties(subStrings);
                break;
            // Info: Resets your kd
            case "resetkd":
                ResetKd();
                break;
            // Info: Resets kd for all players on the server
            case "resetkdall":
                ResetKdAll();
                break;
            // Info: 
            case "pm":
                SendPrivateMessage(subStrings);
                break;
            // Info: Switch team
            case "team":
                var team = subStrings[1];
                SwitchToTeam(team);
                break;
            // Info: Restarts the server
            case "restart":
                RestartGame();
                break;
            // Info: 
            case "specmode":
                EnterExitSpecMode();
                break;
            // Info: 
            case "fov":
                var fov = subStrings[1];
                SetFov(fov);
                break;
            // Info: 
            case "colliders":
                OutputCollisions();
                break;
            // Info: 
            case "spectate":
                var playerId = Convert.ToInt32(input.Substring(10));
                SpectatePlayer(playerId);
                break;
            // Info: 
            case "kill":
                break;
            // Info: 
            case "revive":
                playerId = Convert.ToInt32(inputLine.Substring(8));
                RevievePlayer(playerId);
                break;
            // Info: 
            case "reviveall":
                ReviveAllPlayers();
                    break;
            // Info: 
            case "unban":
                UnbanPlayer(input);
                break;
            // Info: 
            case "rules":
                OutputRules();
                break;
            // Info: 
            case "kick":
                playerId = Convert.ToInt32(input.Substring(6));
                KickPlayer(playerId);
                break;
            // Info: 
            case "ban":
                playerId = Convert.ToInt32(input.Substring(5));
                BanPlayer(playerId);
                break;
            // Info: 
            case "banlist":
                OutputBanList();
                break;
            case "clear":
                ClearChat();
                    break;
            default:
                break;
        }
    }
}