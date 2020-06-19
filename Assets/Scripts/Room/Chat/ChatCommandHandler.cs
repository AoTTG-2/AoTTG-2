using Assets.Scripts.Characters.Titan;
using Assets.Scripts.Gamemode.Options;
using ExitGames.Client.Photon;
using System;
using UnityEngine;
using static PhotonNetwork;
using static FengGameManagerMKII;
using static ChatUtility;
using System.Linq;
using System.Text;

/// <summary>
/// Handles logic for server chat commands
/// </summary>
public static class ChatCommandHandler
{
    /// <summary>
    /// Outputs list of banned players to chat
    /// </summary>
    private static void OutputBanList()
    {
        var message = ChatUtility.FormatSystemMessage("List of banned players:");
        instance.chatRoom.AddMessage(message);
        foreach (int key in banHash.Keys)
        {
            message = ChatUtility.FormatSystemMessage($"{key}:{banHash[key]}");
            instance.chatRoom.AddMessage(message);
        }
    }

    /// <summary>
    /// Ban player with Id
    /// </summary>
    /// <param name="playerIdString"></param>
    private static void BanPlayer(string playerIdString)
    {
        int playerId;
        if (int.TryParse(playerIdString, out playerId))
        {
            if (playerId == player.ID)
            {
                instance.chatRoom.OutputErrorMessage("You can't ban yourself.");
            }
            else if (!(OnPrivateServer || isMasterClient))
            {
                var chatMessage = new object[] { $"/ban #{playerId}", LoginFengKAI.player.name };
                instance.photonView.RPC("Chat", PhotonTargets.All, chatMessage);
            }
            else
            {
                var playerToBan = playerList.FirstOrDefault(p => p.ID == playerId);

                if (!playerList.Any(p => p.ID == playerId))
                {
                    instance.chatRoom.OutputErrorPlayerNotFound(playerIdString);
                    return;
                }

                instance.kickPlayerRC(playerToBan, true, string.Empty);
                var chatMessage = new object[] { FormatSystemMessage($"{GetPlayerName(playerToBan)} has been banned from the server!"), string.Empty };
                instance.photonView.RPC("Chat", PhotonTargets.All, chatMessage);
            }
        }
        else
        {
            instance.chatRoom.OutputErrorPlayerNotFound(playerIdString);
        }
    }

    /// <summary>
    /// Kick Player with Id
    /// </summary>
    /// <param name="playerIdString"></param>
    private static void KickPlayer(string playerIdString)
    {
        int playerId;
        if (int.TryParse(playerIdString, out playerId))
        {
            if (playerId == player.ID)
            {
                instance.chatRoom.OutputErrorMessage("You can't kick yourself.");
            }
            else if (!(OnPrivateServer || isMasterClient))
            {
                var chatMessage = new object[] { $"/kick #{playerId}", LoginFengKAI.player.name };
                instance.photonView.RPC("Chat", PhotonTargets.All, chatMessage);
            }
            else
            {
                var playerToKick = playerList.FirstOrDefault(p => p.ID == playerId);

                if (playerToKick == null)
                {
                    instance.chatRoom.OutputErrorPlayerNotFound(playerIdString);
                    return;
                }
                if (OnPrivateServer)
                {
                    instance.kickPlayerRC(playerToKick, false, string.Empty);
                }
                else if (isMasterClient)
                {
                    instance.kickPlayerRC(playerToKick, false, string.Empty);
                    var chatMessage = new object[] { FormatSystemMessage($"{GetPlayerName(playerToKick)} has been kicked from the server!"), string.Empty };
                    instance.photonView.RPC("Chat", PhotonTargets.All, chatMessage);
                }

            }
        }
        else
        {
            instance.chatRoom.OutputErrorPlayerNotFound(playerIdString);
        }
    }

    /// <summary>
    /// Outputs server rules to chat
    /// </summary>
    private static void OutputRules()
    {
        var message = FormatSystemMessage("Currently activated gamemodes:");
        instance.chatRoom.AddMessage(message);
        if (FengGameManagerMKII.Gamemode.Settings.Horse)
        {
            message = FormatSystemMessage("Horses are enabled.");
            instance.chatRoom.AddMessage(message);
        }
        if (FengGameManagerMKII.Gamemode.Settings.Motd != string.Empty)
        {
            message = FormatSystemMessage($"MOTD: {FengGameManagerMKII.Gamemode.Settings.Motd}");
            instance.chatRoom.AddMessage(message);
        }
    }

    /// <summary>
    /// Unban player with Id
    /// </summary>
    /// <param name="playerId"></param>
    private static void UnbanPlayer(string playerId)
    {
        if (OnPrivateServer)
        {
            ServerRequestUnban(playerId);
        }
        else if (isMasterClient)
        {
            int key;
            if (int.TryParse(playerId, out key))
            {
                if (banHash.ContainsKey(key))
                {
                    var chatMessage = new object[] { $"{banHash[key]} has been unbanned from the server.", string.Empty };
                    instance.photonView.RPC("Chat", PhotonTargets.All, chatMessage);
                    banHash.Remove(key);
                }
                else
                {
                    instance.chatRoom.OutputErrorPlayerNotFound(key.ToString());
                }
            }
            else
            {
                instance.chatRoom.OutputErrorPlayerNotFound(playerId);
            }
        }
        else
        {
            instance.chatRoom.OutputErrorNotMasterClient();
        }
    }

    /// <summary>
    /// Revive all players
    /// </summary>
    private static void ReviveAllPlayers()
    {
        if (isMasterClient)
        {
            var chatMessage = new object[] { FormatSystemMessage("All players have been revived."), string.Empty };
            instance.photonView.RPC("Chat", PhotonTargets.All, chatMessage);
            foreach (PhotonPlayer player in playerList)
            {
                if ((player.CustomProperties[PhotonPlayerProperty.dead] != null) && RCextensions.returnBoolFromObject(player.CustomProperties[PhotonPlayerProperty.dead]) && (RCextensions.returnIntFromObject(player.CustomProperties[PhotonPlayerProperty.isTitan]) != 2))
                {
                    instance.photonView.RPC("respawnHeroInNewRound", player, new object[0]);
                }
            }
        }
    }

    /// <summary>
    /// Revive player with Id
    /// </summary>
    /// <param name="playerIdString"></param>
    private static void RevievePlayer(string playerIdString)
    {
        if (isMasterClient)
        {
            int playerId;
            if (int.TryParse(playerIdString, out playerId))
            {
                var player = playerList.FirstOrDefault(p => p.ID == playerId);

                if (playerList.Any(p => p.ID == playerId))
                {
                    var message = FormatSystemMessage($"Player {playerId} has been revived.");
                    instance.chatRoom.AddMessage(message);
                    instance.photonView.RPC("RespawnRpc", player);
                }
            }
            else
            {
                instance.chatRoom.OutputErrorPlayerNotFound(playerIdString);
            }
        }
        else
        {
            instance.chatRoom.OutputErrorNotMasterClient();
        }
    }

    /// <summary>
    /// Spectate player with Id
    /// </summary>
    /// <param name="playerIdString"></param>
    private static void SpectatePlayer(string playerIdString)
    {
        int playerId;
        if (int.TryParse(playerIdString, out playerId))
        {
            foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("Player"))
            {
                if (gameObject.GetPhotonView().owner.ID == playerId)
                {
                    Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().setMainObject(gameObject, true, false);
                    Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().setSpectorMode(false);
                }
            }
        }
        else
        {
            instance.chatRoom.OutputErrorPlayerNotFound(playerIdString);
        }
            
    }

    /// <summary>
    /// Output number of titan Collisions
    /// </summary>
    private static void OutputCollisions()
    {
        int collisions = 0;
        foreach (MindlessTitan titan in instance.getTitans())
        {
            if (titan.IsColliding)
            {
                collisions++;
            }
        }
        var message = collisions.ToString();
        instance.chatRoom.AddMessage(message);
    }

    /// <summary>
    /// Set field of view
    /// </summary>
    /// <param name="input"></param>
    private static void SetFov(string input)
    {
        int fov;
        if (int.TryParse(input, out fov))
        {
            Camera.main.fieldOfView = fov;
            var message = $"Field of vision set to {fov}.";
            instance.chatRoom.AddMessage(message);
        }
        else
        {
            instance.chatRoom.OutputErrorMessage("Fov has to be a number");
        }
    }

    private static void ToggleSpecMode()
    {
        settings[0xf5] = (int)settings[0xf5] == 1 ? 0 : 1;
        bool specMode = (int)settings[0xf5] == 1;
        instance.EnterSpecMode(specMode);
        string message = FormatSystemMessage(specMode ? "You have entered spectator mode." : "You have exited spectator mode.");
        instance.chatRoom.AddMessage(message);
    }

    /// <summary>
    /// Restart current game
    /// </summary>
    private static void RestartGame()
    {
        if (isMasterClient)
        {
            var chatMessage = new object[] { FormatSystemMessage("MasterClient has restarted the game!"), string.Empty };
            instance.photonView.RPC("Chat", PhotonTargets.All, chatMessage);
            instance.restartRC();
        }
        else
        {
            instance.chatRoom.OutputErrorNotMasterClient();
        }
    }

    private static void SendPrivateMessage(string[] parameters)
    {
        int playerId;
        var playerIdString = parameters[1];
        if (int.TryParse(playerIdString, out playerId))
        {
            var targetPlayer = PhotonPlayer.Find(playerId);

            StringBuilder chatMessage = new StringBuilder();
            for (var messageIndex = 2; messageIndex < parameters.Length; messageIndex++)
            {
                chatMessage.Append(parameters[messageIndex] + " ");
            }
            instance.photonView.RPC("ChatPM", targetPlayer, new object[] { SetNameColorDependingOnteam(player), chatMessage.ToString() });

            var message = $"TO [{targetPlayer.ID}] {SetNameColorDependingOnteam(targetPlayer)}:{chatMessage}";
            instance.chatRoom.AddMessage(message);
        }
        else
        {
            instance.chatRoom.OutputErrorPlayerNotFound(playerIdString);
        }      
    }

    /// <summary>
    /// Reset your kd
    /// </summary>
    private static void ResetKd()
    {
        var hashTable = new Hashtable();
        hashTable.Add(PhotonPlayerProperty.kills, 0);
        hashTable.Add(PhotonPlayerProperty.deaths, 0);
        hashTable.Add(PhotonPlayerProperty.max_dmg, 0);
        hashTable.Add(PhotonPlayerProperty.total_dmg, 0);
        player.SetCustomProperties(hashTable);

        var message = "Your stats have been reset.";
        instance.chatRoom.AddMessage(message);
    }

    private static void ChangeRoomProperties(string[] input)
    {
        if (isMasterClient)
        {
            ChatCommand command;
            if (input.Count() >= 3)
            {
                if (Enum.TryParse(input[1], out command))
                {
                    string parameter = input[2];
                    int maxPlayers;
                    float time;

                    switch (command)
                    {
                        case ChatCommand.max:
                            if (int.TryParse(parameter, out maxPlayers))
                            {
                                ChangeRoomMaxPlayers(maxPlayers);
                            }
                            else
                            {
                                instance.chatRoom.OutputErrorMessage($"{parameter} is not a number.");
                            }
                            break;
                        case ChatCommand.time:
                            if (float.TryParse(parameter, out time))
                            {
                                AddPlayTime(time);
                            }
                            else
                            {
                                instance.chatRoom.OutputErrorMessage("Time to add must be a number.");
                            }
                            break;
                    } 
                }
                else
                {
                    instance.chatRoom.OutputErrorMessage("Valid room attributes are max or time");
                }
            }
        }
        else
        {
            instance.chatRoom.OutputErrorNotMasterClient();
        }
    }
    private static void ChangeRoomMaxPlayers(int maxPlayers)
    {
        instance.maxPlayers = maxPlayers;
        room.MaxPlayers = maxPlayers;
        var chatMessage = new object[] { FormatSystemMessage($"Max players changed to {maxPlayers}!"), string.Empty };
        instance.photonView.RPC("Chat", PhotonTargets.All, chatMessage);
    }

    private static void AddPlayTime(float time)
    {
        instance.addTime(time);
        var chatMessage = new object[] { FormatSystemMessage($"{time} seconds added to the clock."), string.Empty };
        instance.photonView.RPC("Chat", PhotonTargets.All, chatMessage);
    }

    private static void OutputIgnoreList()
    {
        foreach (var ignoredPlayer in ignoreList)
        {
            var message = $"{ignoredPlayer}";
            instance.chatRoom.AddMessage(message);
        }
    }

    private static void OutputIsRc()
    {
        var message = masterRC ? "Is RC" : "Not RC";
        instance.chatRoom.AddMessage(message);
    }

    private static void CheckLevel(PhotonPlayer player)
    {
        var message = RCextensions.returnStringFromObject(player.CustomProperties[PhotonPlayerProperty.currentLevel]);
        instance.chatRoom.AddMessage(message);
    }
        
    private static void UnPauseGame()
    {
        if (isMasterClient)
        {
            instance.photonView.RPC("pauseRPC", PhotonTargets.All, new object[] { false });
            var chatMessage = new object[] { FormatSystemMessage("MasterClient has unpaused the game."), string.Empty };
            instance.photonView.RPC("Chat", PhotonTargets.All, chatMessage);
        }
        else
        {
            instance.chatRoom.OutputErrorNotMasterClient();
        }
    }

    private static void PauseGame()
    {
        if (isMasterClient)
        {
            instance.photonView.RPC("pauseRPC", PhotonTargets.All, new object[] { true });
            var chatMessage = new object[] { FormatSystemMessage("MasterClient has paused the game."), string.Empty };
            instance.photonView.RPC("Chat", PhotonTargets.All, chatMessage);
        }
        else
        {
            instance.chatRoom.OutputErrorNotMasterClient();
        }
    }

    /// <summary>
    /// turn on/off preserve KDR on diconnect
    /// </summary>
    /// <param name="command"></param>
    private static void PreserveKdrOnOFF(string parameter)
    {
        string message;
        if (isMasterClient)
        {
            ChatCommand command;
            if (Enum.TryParse(parameter, out command))
            {
                switch (command)
                {
                    // Info: 
                    case ChatCommand.kdr:
                        if (!FengGameManagerMKII.Gamemode.Settings.SaveKDROnDisconnect)
                        {
                            FengGameManagerMKII.Gamemode.Settings.SaveKDROnDisconnect = true;
                            message = "KDRs will be preserved from disconnects.";
                        }
                        else
                        {
                            FengGameManagerMKII.Gamemode.Settings.SaveKDROnDisconnect = false;
                            message = "KDRs will not be preserved from disconnects.";
                        }

                        instance.chatRoom.AddMessage(message);
                        break;
                }
            }
        }
        else
        {
            instance.chatRoom.OutputErrorNotMasterClient();
        }
    }

    /// <summary>
    /// Reset KD for all players on the server
    /// </summary>
    private static void ResetKdAll()
    {
        if (isMasterClient)
        {
            foreach (PhotonPlayer player in playerList)
            {
                var hashTable = new Hashtable();
                hashTable.Add(PhotonPlayerProperty.kills, 0);
                hashTable.Add(PhotonPlayerProperty.deaths, 0);
                hashTable.Add(PhotonPlayerProperty.max_dmg, 0);
                hashTable.Add(PhotonPlayerProperty.total_dmg, 0);
                player.SetCustomProperties(hashTable);
            }
            var chatMessage = new object[] { FormatSystemMessage("All stats have been reset."), string.Empty };
            instance.photonView.RPC("Chat", PhotonTargets.All, chatMessage);
        }
        else
        {
            instance.chatRoom.OutputErrorNotMasterClient();
        }
    }

    /// <summary>
    /// Switch to team
    /// </summary>
    /// <param name="team"></param>
    private static void SwitchToTeam(string team)
    {
        ChatCommand teamEnum;
        string message = string.Empty;
        if (Gamemode.Settings.TeamMode == TeamMode.NoSort)
        {
            if (Enum.TryParse(team, out teamEnum))
            {
                switch (teamEnum)
                {
                    case ChatCommand.none:
                        SwitchTeam((int)teamEnum);
                        message = FormatTextColor00FFFF("You have joined team cyan.");
                        break;
                    case ChatCommand.cyan:
                        SwitchTeam((int)teamEnum);
                        message = FormatTextColor00FFFF("You have joined team cyan.");
                        break;
                    case ChatCommand.magenta:
                        SwitchTeam((int)teamEnum);
                        message = FormatTextColorFF00FF("You have joined team magenta.");
                        break;
                    default:
                        instance.chatRoom.OutputErrorMessage("Invalid team name. Accepted text values are cyan or magenta.");
                        break;
                }
            }

            instance.chatRoom.AddMessage(message);
        }
        else
        {
            instance.chatRoom.OutputErrorMessage("Teams are locked or disabled.");
        }
    }

    /// <summary>
    /// Clear all messages in chat
    /// </summary>
    private static void ClearChat()
    {
        if (isMasterClient)
        {
            instance.chatRoom.ClearMessages();
        }
        else
        {
            instance.chatRoom.OutputErrorNotMasterClient();
        }
    }

    /// <summary>
    /// Switch to team
    /// </summary>
    /// <param name="team"></param>
    private static void SwitchTeam(int team)
    {
        instance.photonView.RPC("setTeamRPC", player, new object[] { team });
        foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (gameObject.GetPhotonView().isMine)
            {
                gameObject.GetComponent<Hero>().markDie();
                gameObject.GetComponent<Hero>().photonView.RPC("netDie2", PhotonTargets.All, new object[] { -1, "Team Switch" });
            }
        }
    }

    /// <summary>
    /// Handle commands in chat
    /// </summary>
    /// <param name="chatCommand"></param>
    public static void CommandHandler(string chatCommand)
    {
        var commands = chatCommand.Replace("/", "").Split(' ');
        var parameter = string.Empty;

        ChatCommand command;

        if (Enum.TryParse(commands[0].ToLower(), out command))
        {
            if (commands.Count() > 1)
            {
                parameter = commands[1];
            }

            string message;
            switch (command)
            {
                // Info: 
                case ChatCommand.cloth:
                    message = ClothFactory.GetDebugInfo();
                    instance.chatRoom.AddMessage(message);
                    break;
                // Info: 
                case ChatCommand.aso:
                    PreserveKdrOnOFF(parameter);
                    break;
                // Info: Pauses the game
                case ChatCommand.pause:
                    PauseGame();
                    break;
                // Info: Unpauses the game
                case ChatCommand.unpause:
                    UnPauseGame();
                    break;
                // Info: Outputs name of level
                case ChatCommand.checklevel:
                    CheckLevel(player);
                    break;
                // Info: 
                case ChatCommand.isrc:
                    OutputIsRc();
                    break;
                // Info: 
                case ChatCommand.ignorelist:
                    OutputIgnoreList();
                    break;
                // Info: 
                case ChatCommand.room:
                    ChangeRoomProperties(commands);
                    break;
                // Info: Resets your kd
                case ChatCommand.resetkd:
                    ResetKd();
                    break;
                // Info: Resets kd for all players on the server
                case ChatCommand.resetkdall:
                    ResetKdAll();
                    break;
                // Info: 
                case ChatCommand.pm:
                    SendPrivateMessage(commands);
                    break;
                // Info: Switch team
                case ChatCommand.team:
                    SwitchToTeam(parameter);
                    break;
                // Info: Restarts the server
                case ChatCommand.restart:
                    RestartGame();
                    break;
                // Info: 
                case ChatCommand.specmode:
                    ToggleSpecMode();
                    break;
                // Info: 
                case ChatCommand.fov:
                    SetFov(parameter);
                    break;
                // Info: 
                case ChatCommand.colliders:
                    OutputCollisions();
                    break;
                // Info: 
                case ChatCommand.spectate:
                    SpectatePlayer(parameter);
                    break;
                // Info: 
                case ChatCommand.revive:
                    RevievePlayer(parameter);
                    break;
                // Info: 
                case ChatCommand.reviveall:
                    ReviveAllPlayers();
                    break;
                // Info: 
                case ChatCommand.unban:
                    UnbanPlayer(parameter);
                    break;
                // Info: 
                case ChatCommand.rules:
                    OutputRules();
                    break;
                // Info: 
                case ChatCommand.kick:
                    KickPlayer(parameter);
                    break;
                // Info: 
                case ChatCommand.ban:
                    BanPlayer(parameter);
                    break;
                // Info: 
                case ChatCommand.banlist:
                    OutputBanList();
                    break;
                case ChatCommand.clear:
                    ClearChat();
                    break;
                default:
                    break;
            }
        }
    }
}
