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
using WebSocketSharp;
using System.Linq;

public class InRoomChat : Photon.MonoBehaviour
{
    private bool AlignBottom = true;
    public static readonly string ChatRPC = "Chat";
    private string inputLine = string.Empty;
    public bool IsVisible = true;
    private readonly List<string> messages = new List<string>();
    private Vector2 scrollPos = Vector2.zero;
    public InputField ChatInputField;
    public Text ChatText;

    /// <summary>
    /// Adds message to message list
    /// </summary>
    /// <param name="message"></param>
    public void AddMessage(string message)
    {
        if (message.Count() <= 1000)
        {
            messages.Add(message);
        }
    }

    /// <summary>
    /// Formats text as <color=#00FFFF>{input}</color>
    /// </summary>
    /// <param name="input"></param>
    /// <returns><color=#00FFFF>{input}</color></returns>
    private string FormatTextColor00FFFF(string input)
    {
        return $"<color=#00FFFF>{input}</color>";
    }

    /// <summary>
    /// Formats text as <color=#FF00FF>{input}</color>
    /// </summary>
    /// <param name="input"></param>
    /// <returns><color=#FF00FF>{input}</color></returns>
    private string FormatTextColorFF00FF(string input)
    {
        return $"<color=#FF00FF>{input}</color>";
    }

    /// <summary>
    /// Formats text as <color=#FFCC00>{input}</color>
    /// </summary>
    /// <param name="input"></param>
    /// <returns><color=#FFCC00>{input}</color></returns>
    private string FormatTextColorSystemMessage(string input)
    {
        return $"<color=#FFCC00>{input}</color>";
    }

    /// <summary>
    /// Formats text as <color=#FFCC00>{message}</color>
    /// </summary>
    /// <param name="message"></param>
    private void AddSystemMessage(string message)
    {
        AddMessage($"<color=#FFCC00>{message}</color>");
    }

    /// <summary>
    /// Outputs Error: Not Master Client. in chat
    /// </summary>
    private void AddMessageErrorNotMasterClient()
    {
        AddMessage($"<color=#FFCC00>Error: Not Master Client.</color>");
    }

    /// <summary>
    /// Gets the name of PhotonPlayer.player
    /// </summary>
    /// <param name="player"></param>
    /// <returns>PhotonPlayerProperty.name</returns>
    private string GetPlayerName(PhotonPlayer player)
    {
        return RCextensions.returnStringFromObject(player.CustomProperties[PhotonPlayerProperty.name]);
    }

    /// <summary>
    /// sets color of name in chat depending on what team PhotonPlayer.player is in
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    private string SetNameColorDependingOnteam(PhotonPlayer player)
    {
        string result = "";
        var name = GetPlayerName(player);
        if (name.hexColor().IsNullOrEmpty())
        {
            if (player.CustomProperties[PhotonPlayerProperty.RCteam] != null)
            {

                if (RCextensions.returnIntFromObject(player.CustomProperties[PhotonPlayerProperty.RCteam]) == 1)
                {
                    result = FormatTextColor00FFFF(name);
                }
                else if (RCextensions.returnIntFromObject(player.CustomProperties[PhotonPlayerProperty.RCteam]) == 2)
                {
                    result = FormatTextColorFF00FF(name);
                }
            }
        }

        return result;
    }

    public void OnGUI()
    {
        int i;
        if (!IsVisible || (connectionState != ConnectionState.Connected))
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
            if (!inputLine.IsNullOrEmpty())
            {
                if (inputLine == "\t")
                {
                    inputLine = string.Empty;
                    ChatInputField.gameObject.GetComponent<Text>().text = string.Empty;
                    return;
                }
                if (RCEvents.ContainsKey("OnChatInput"))
                {
                    string key = (string)RCVariableNames["OnChatInput"];
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
                    var parameters = new object[] { inputLine, SetNameColorDependingOnteam(player) };
                    instance.photonView.RPC("Chat", PhotonTargets.All, parameters);
                }
                else if (inputLine == "/cloth")
                {
                    AddMessage(ClothFactory.GetDebugInfo());
                }
                else if (inputLine.StartsWith("/aso"))
                {
                    if (isMasterClient)
                    {
                        switch (inputLine.Substring(5))
                        {
                            case "kdr":
                                if (!Gamemode.Settings.SaveKDROnDisconnect)
                                {
                                    Gamemode.Settings.SaveKDROnDisconnect = true;
                                    AddSystemMessage("KDRs will be preserved from disconnects.");
                                }
                                else
                                {
                                    Gamemode.Settings.SaveKDROnDisconnect = false;
                                    AddSystemMessage("KDRs will not be preserved from disconnects.");
                                }
                                break;
                        }
                    }
                }
                else
                {
                    if (inputLine == "/pause")
                    {
                        if (isMasterClient)
                        {
                            instance.photonView.RPC("pauseRPC", PhotonTargets.All, new object[] { true });
                            var objArray3 = new object[] { FormatTextColorSystemMessage("MasterClient has paused the game."), "" };
                            instance.photonView.RPC("Chat", PhotonTargets.All, objArray3);
                        }
                        else
                        {
                            AddMessageErrorNotMasterClient();
                        }
                    }
                    else if (inputLine == "/unpause")
                    {
                        if (isMasterClient)
                        {
                            instance.photonView.RPC("pauseRPC", PhotonTargets.All, new object[] { false });
                            var objArray3 = new object[] { FormatTextColorSystemMessage("MasterClient has unpaused the game."), "" };
                            instance.photonView.RPC("Chat", PhotonTargets.All, objArray3);
                        }
                        else
                        {
                            AddMessageErrorNotMasterClient();
                        }
                    }
                    else if (inputLine == "/checklevel")
                    {
                        foreach (PhotonPlayer player in playerList)
                        {
                            AddMessage(RCextensions.returnStringFromObject(player.CustomProperties[PhotonPlayerProperty.currentLevel]));
                        }
                    }
                    else if (inputLine == "/isrc")
                    {
                        if (masterRC)
                        {
                            AddMessage("is RC");
                        }
                        else
                        {
                            AddMessage("not RC");
                        }
                    }
                    else if (inputLine == "/ignorelist")
                    {
                        foreach (var ignore in ignoreList)
                        {
                            AddMessage($"{ignore}");
                        }
                    }
                    else if (inputLine.StartsWith("/room"))
                    {
                        if (isMasterClient)
                        {
                            if (inputLine.Substring(6).StartsWith("max"))
                            {
                                int maxPlayers = Convert.ToInt32(inputLine.Substring(10));
                                instance.maxPlayers = maxPlayers;
                                room.MaxPlayers = maxPlayers;
                                var objArray3 = new object[] { FormatTextColorSystemMessage($"Max players changed to {inputLine.Substring(10)}!"), "" };
                                instance.photonView.RPC("Chat", PhotonTargets.All, objArray3);
                            }
                            else if (inputLine.Substring(6).StartsWith("time"))
                            {
                                instance.addTime(Convert.ToSingle(inputLine.Substring(11)));
                                var objArray3 = new object[] { FormatTextColorSystemMessage($"{inputLine.Substring(11)} seconds added to the clock."), "" };
                                instance.photonView.RPC("Chat", PhotonTargets.All, objArray3);
                            }
                        }
                        else
                        {
                            AddMessageErrorNotMasterClient();
                        }
                    }
                    else if (inputLine.StartsWith("/resetkd"))
                    {
                        Hashtable hashtable;
                        if (inputLine == "/resetkdall")
                        {
                            if (isMasterClient)
                            {
                                foreach (PhotonPlayer player in playerList)
                                {
                                    hashtable = new Hashtable();
                                    hashtable.Add(PhotonPlayerProperty.kills, 0);
                                    hashtable.Add(PhotonPlayerProperty.deaths, 0);
                                    hashtable.Add(PhotonPlayerProperty.max_dmg, 0);
                                    hashtable.Add(PhotonPlayerProperty.total_dmg, 0);
                                    player.SetCustomProperties(hashtable);
                                }
                                var objArray3 = new object[] { FormatTextColorSystemMessage("All stats have been reset."), "" };
                                instance.photonView.RPC("Chat", PhotonTargets.All, objArray3);
                            }
                            else
                            {
                                AddMessageErrorNotMasterClient();
                            }
                        }
                        else
                        {
                            hashtable = new Hashtable();
                            hashtable.Add(PhotonPlayerProperty.kills, 0);
                            hashtable.Add(PhotonPlayerProperty.deaths, 0);
                            hashtable.Add(PhotonPlayerProperty.max_dmg, 0);
                            hashtable.Add(PhotonPlayerProperty.total_dmg, 0);
                            player.SetCustomProperties(hashtable);
                            AddSystemMessage("Your stats have been reset.");
                        }
                    }
                    else if (inputLine.StartsWith("/pm"))
                    {
                        var inputPlayerName = inputLine.Split(new char[] { ' ' });
                        PhotonPlayer targetPlayer = PhotonPlayer.Find(Convert.ToInt32(inputPlayerName[1]));

                        string str4 = string.Empty;
                        for (var nameIndex = 2; nameIndex < inputPlayerName.Length; nameIndex++)
                        {
                            str4 = str4 + inputPlayerName[nameIndex] + " ";
                        }
                        instance.photonView.RPC("ChatPM", targetPlayer, new object[] { SetNameColorDependingOnteam(player), str4 });
                        AddSystemMessage($"TO [{targetPlayer.ID}] {SetNameColorDependingOnteam(targetPlayer)}:{str4}");
                    }
                    else if (inputLine.StartsWith("/team"))
                    {
                        if (Gamemode.Settings.TeamMode == TeamMode.NoSort)
                        {
                            if ((inputLine.Substring(6) == "1") || (inputLine.Substring(6) == "cyan"))
                            {
                                instance.photonView.RPC("setTeamRPC", player, new object[] { 1 });
                                AddMessage(FormatTextColor00FFFF("You have joined team cyan."));
                                foreach (GameObject obj2 in GameObject.FindGameObjectsWithTag("Player"))
                                {
                                    if (obj2.GetPhotonView().isMine)
                                    {
                                        obj2.GetComponent<Hero>().markDie();
                                        obj2.GetComponent<Hero>().photonView.RPC("netDie2", PhotonTargets.All, new object[] { -1, "Team Switch" });
                                    }
                                }
                            }
                            else if ((inputLine.Substring(6) == "2") || (inputLine.Substring(6) == "magenta"))
                            {
                                instance.photonView.RPC("setTeamRPC", player, new object[] { 2 });
                                AddMessage(FormatTextColor00FFFF("You have joined team magenta."));
                                foreach (GameObject obj3 in GameObject.FindGameObjectsWithTag("Player"))
                                {
                                    if (obj3.GetPhotonView().isMine)
                                    {
                                        obj3.GetComponent<Hero>().markDie();
                                        obj3.GetComponent<Hero>().photonView.RPC("netDie2", PhotonTargets.All, new object[] { -1, "Team Switch" });
                                    }
                                }
                            }
                            else if ((inputLine.Substring(6) == "0") || (inputLine.Substring(6) == "individual"))
                            {
                                instance.photonView.RPC("setTeamRPC", player, new object[] { 0 });
                                AddMessage(FormatTextColor00FFFF("You have joined the individuals."));
                                foreach (GameObject obj4 in GameObject.FindGameObjectsWithTag("Player"))
                                {
                                    if (obj4.GetPhotonView().isMine)
                                    {
                                        obj4.GetComponent<Hero>().markDie();
                                        obj4.GetComponent<Hero>().photonView.RPC("netDie2", PhotonTargets.All, new object[] { -1, "Team Switch" });
                                    }
                                }
                            }
                            else
                            {
                                AddSystemMessage("error: invalid team code. Accepted values are 0,1, and 2.");
                            }
                        }
                        else
                        {
                            AddSystemMessage("error: teams are locked or disabled.");
                        }
                    }
                    else if (inputLine == "/restart")
                    {
                        if (isMasterClient)
                        {
                            var objArray3 = new object[] { FormatTextColorSystemMessage("MasterClient has restarted the game!"), "" };
                            instance.photonView.RPC("Chat", PhotonTargets.All, objArray3);
                            instance.restartRC();
                        }
                        else
                        {
                            AddMessageErrorNotMasterClient();
                        }
                    }
                    else if (inputLine.StartsWith("/specmode"))
                    {
                        if (((int)settings[0xf5]) == 0)
                        {
                            settings[0xf5] = 1;
                            instance.EnterSpecMode(true);
                            AddSystemMessage("You have entered spectator mode.");
                        }
                        else
                        {
                            settings[0xf5] = 0;
                            instance.EnterSpecMode(false);
                            AddSystemMessage("You have exited spectator mode.");
                        }
                    }
                    else if (inputLine.StartsWith("/fov"))
                    {
                        int inputFieldOfVision = Convert.ToInt32(inputLine.Substring(5));
                        Camera.main.fieldOfView = inputFieldOfVision;
                        AddSystemMessage($"Field of vision set to {inputFieldOfVision}.");
                    }
                    else if (inputLine == "/colliders")
                    {
                        int num7 = 0;
                        foreach (MindlessTitan titan in instance.getTitans())
                        {
                            if (titan.IsColliding)
                            {
                                num7++;
                            }
                        }
                        instance.chatRoom.AddMessage(num7.ToString());
                    }
                    else
                    {
                        if (inputLine.StartsWith("/spectate"))
                        {
                            var playerNameIndex = Convert.ToInt32(inputLine.Substring(10));
                            foreach (GameObject obj5 in GameObject.FindGameObjectsWithTag("Player"))
                            {
                                if (obj5.GetPhotonView().owner.ID == playerNameIndex)
                                {
                                    Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().setMainObject(obj5, true, false);
                                    Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().setSpectorMode(false);
                                }
                            }
                        }
                        else if (!inputLine.StartsWith("/kill"))
                        {
                            object[] objArray5;
                            if (inputLine.StartsWith("/revive"))
                            {
                                if (isMasterClient)
                                {
                                    if (inputLine == "/reviveall")
                                    {
                                        objArray5 = new object[] { FormatTextColorSystemMessage("All players have been revived."), string.Empty };
                                        instance.photonView.RPC("Chat", PhotonTargets.All, objArray5);
                                        foreach (PhotonPlayer player in playerList)
                                        {
                                            if (((player.CustomProperties[PhotonPlayerProperty.dead] != null) && RCextensions.returnBoolFromObject(player.CustomProperties[PhotonPlayerProperty.dead])) && (RCextensions.returnIntFromObject(player.CustomProperties[PhotonPlayerProperty.isTitan]) != 2))
                                            {
                                                instance.photonView.RPC("respawnHeroInNewRound", player, new object[0]);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        var playerId = Convert.ToInt32(inputLine.Substring(8));
                                        foreach (PhotonPlayer player in playerList)
                                        {
                                            if (player.ID == playerId)
                                            {
                                                AddSystemMessage($"Player {playerId} has been revived.");
                                                instance.photonView.RPC("RespawnRpc", player);
                                                return;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    AddMessageErrorNotMasterClient();
                                }
                            }
                            else if (inputLine.StartsWith("/unban"))
                            {
                                if (OnPrivateServer)
                                {
                                    ServerRequestUnban(inputLine.Substring(7));
                                }
                                else if (isMasterClient)
                                {
                                    var hash = Convert.ToInt32(inputLine.Substring(7));
                                    if (banHash.ContainsKey(hash))
                                    {
                                        objArray5 = new object[] { $"{banHash[hash]} has been unbanned from the server.", string.Empty };
                                        instance.photonView.RPC("Chat", PhotonTargets.All, objArray5);
                                        banHash.Remove(hash);
                                    }
                                    else
                                    {
                                        AddSystemMessage("error: no such player");
                                    }
                                }
                                else
                                {
                                    AddMessageErrorNotMasterClient();
                                }
                            }
                            else if (inputLine.StartsWith("/rules"))
                            {
                                AddSystemMessage("Currently activated gamemodes:");
                                if (Gamemode.Settings.Horse)
                                {
                                    AddSystemMessage("Horses are enabled.");
                                }
                                if (Gamemode.Settings.Motd != string.Empty)
                                {
                                    AddSystemMessage($"MOTD: {Gamemode.Settings.Motd}");
                                }
                            }
                            else
                            {
                                if (inputLine.StartsWith("/kick"))
                                {
                                    var playerId = Convert.ToInt32(inputLine.Substring(6));
                                    if (playerId == player.ID)
                                    {
                                        AddSystemMessage("error:can't kick yourself.");
                                    }
                                    else if (!(OnPrivateServer || isMasterClient))
                                    {
                                        var objArray6 = new object[] { $"/kick #{playerId}", LoginFengKAI.player.name };
                                        instance.photonView.RPC("Chat", PhotonTargets.All, objArray6);
                                    }
                                    else
                                    {
                                        var playerToKick = playerList.FirstOrDefault(p => p.ID == playerId);

                                        if (OnPrivateServer)
                                        {
                                            instance.kickPlayerRC(playerToKick, false, "");
                                        }
                                        else if (isMasterClient)
                                        {
                                            instance.kickPlayerRC(playerToKick, false, "");
                                            var objArray7 = new object[] { $"{GetPlayerName(playerToKick)} has been kicked from the server!", string.Empty };
                                            instance.photonView.RPC("Chat", PhotonTargets.All, objArray7);
                                        }

                                        if (playerToKick == null)
                                        {
                                            AddSystemMessage("error:no such player.");
                                        }
                                    }
                                }
                                else if (inputLine.StartsWith("/ban"))
                                {
                                    if (inputLine == "/banlist")
                                    {
                                        AddSystemMessage("List of banned players:");
                                        foreach (int num10 in banHash.Keys)
                                        {
                                            AddSystemMessage($"{num10}:{banHash[num10]}");
                                        }
                                    }
                                    else
                                    {
                                        var playerId = Convert.ToInt32(inputLine.Substring(5));
                                        if (playerId == player.ID)
                                        {
                                            AddMessage("error:can't ban yourself.");
                                        }
                                        else if (!(OnPrivateServer || isMasterClient))
                                        {
                                            var objArray6 = new object[] { $"/ban #{playerId}", LoginFengKAI.player.name };
                                            instance.photonView.RPC("Chat", PhotonTargets.All, objArray6);
                                        }
                                        else
                                        {
                                            var playerToBan = playerList.FirstOrDefault(p => p.ID == playerId);
                                            if (OnPrivateServer)
                                            {
                                                instance.kickPlayerRC(playerToBan, true, "");
                                            }
                                            else if (isMasterClient)
                                            {
                                                instance.kickPlayerRC(playerToBan, true, "");
                                                var objArray7 = new object[] { FormatTextColorSystemMessage($"{GetPlayerName(playerToBan)} has been banned from the server!"), string.Empty };
                                                instance.photonView.RPC("Chat", PhotonTargets.All, objArray7);
                                            }
                                            if (playerToBan == null)
                                            {
                                                AddSystemMessage("error:no such player.");
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
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
        string text = string.Empty;
        if (messages.Count < 15)
        {
            for (i = 0; i < messages.Count; i++)
            {
                text = text + messages[i] + "\n";
            }
        }
        else
        {
            for (i = messages.Count - 15; i < messages.Count; i++)
            {
                text = text + messages[i] + "\n";
            }
        }

        if (ChatText != null) ChatText.text = text;
        inputLine = ChatInputField?.text;
    }
}

