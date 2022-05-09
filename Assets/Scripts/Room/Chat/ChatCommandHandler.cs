using Assets.Scripts.Characters.Humans;
using Assets.Scripts.Gamemode.Options;
using Assets.Scripts.Services;
using Assets.Scripts.Services.Interface;
using Assets.Scripts.Settings;
using ExitGames.Client.Photon;
using System;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Scripts;
using static Assets.Scripts.FengGameManagerMKII;
using static Assets.Scripts.Room.Chat.ChatUtility;
using static PhotonNetwork;
using Assets.Scripts.Utility;
using Assets.Scripts.Events.Args;
using System.Globalization;

/// <summary>
/// Handles logic for server chat commands
/// </summary>
public static class ChatCommandHandler
{
    private static void OutputBanList()
    {
        var message = "List of banned players:";
        instance.chatRoom.OutputSystemMessage(message);
        foreach (int key in banHash.Keys)
        {
            message = $"{key}:{banHash[key]}";
            instance.chatRoom.OutputSystemMessage(message);
        }
    }

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
                instance.photonView.RPC(nameof(FengGameManagerMKII.Chat), PhotonTargets.All, chatMessage);
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
                instance.photonView.RPC(nameof(FengGameManagerMKII.Chat), PhotonTargets.All, chatMessage);
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
                instance.photonView.RPC(nameof(FengGameManagerMKII.Chat), PhotonTargets.All, chatMessage);
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
                    instance.photonView.RPC(nameof(FengGameManagerMKII.Chat), PhotonTargets.All, chatMessage);
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
        var message = "Currently activated gamemodes:";
        instance.chatRoom.OutputSystemMessage(message);
        if (GameSettings.Horse.Enabled.Value)
        {
            message = "Horses are enabled.";
            instance.chatRoom.OutputSystemMessage(message);
        }
        if (GameSettings.Gamemode.Motd != string.Empty)
        {
            message = $"MOTD: {GameSettings.Gamemode.Motd}";
            instance.chatRoom.OutputSystemMessage(message);
        }
    }

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
                    instance.photonView.RPC(nameof(FengGameManagerMKII.Chat), PhotonTargets.All, chatMessage);
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

    private static void ReviveAllPlayers()
    {
        if (!isMasterClient)
        {
            instance.chatRoom.OutputErrorNotMasterClient();
            return;
        }

        var chatMessage = new object[] { FormatSystemMessage("All players have been revived."), string.Empty };
        instance.photonView.RPC(nameof(FengGameManagerMKII.Chat), PhotonTargets.All, chatMessage);
        foreach (PhotonPlayer player in playerList)
        {
            if ((player.CustomProperties[PhotonPlayerProperty.dead] != null) && RCextensions.returnBoolFromObject(player.CustomProperties[PhotonPlayerProperty.dead]) && (RCextensions.returnIntFromObject(player.CustomProperties[PhotonPlayerProperty.isTitan]) != 2))
            {
                instance.photonView.RPC(nameof(FengGameManagerMKII.respawnHeroInNewRound), player, new object[0]);
            }
        }

    }

    private static void RevivePlayer(string playerIdString)
    {
        if (!isMasterClient)
        {
            instance.chatRoom.OutputErrorNotMasterClient();
            return;
        }

        int playerId;
        if (int.TryParse(playerIdString, out playerId))
        {
            var player = playerList.FirstOrDefault(p => p.ID == playerId);

            if (playerList.Any(p => p.ID == playerId))
            {
                var message = $"Player {playerId} has been revived.";
                instance.chatRoom.OutputSystemMessage(message);
                instance.photonView.RPC(nameof(FengGameManagerMKII.RespawnRpc), player);
            }
        }
        else
        {
            instance.chatRoom.OutputErrorPlayerNotFound(playerIdString);
        }
    }

    private static void SpectatePlayer(string playerIdString)
    {
        int playerId;
        if (int.TryParse(playerIdString, out playerId))
        {
            foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("Player"))
            {
                if (gameObject.GetPhotonView().owner.ID == playerId)
                {
                    Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().SetMainObject(gameObject, true, false);
                    Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().SetSpectorMode(false);
                }
            }
        }
        else
        {
            instance.chatRoom.OutputErrorPlayerNotFound(playerIdString);
        }

    }

    private static void OutputCollisions()
    {
        int collisions = 0;
        //TODO: 160
        //foreach (MindlessTitan titan in instance.getTitans())
        //{
        //    if (titan.IsColliding)
        //    {
        //        collisions++;
        //    }
        //}
        var message = collisions.ToString();
        instance.chatRoom.AddMessage(message);
    }

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
        SpectatorMode.Toggle();
        SpectatorMode.UpdateSpecMode();
        string message = SpectatorMode.IsEnable() ? "You have entered spectator mode." : "You have exited spectator mode.";
        instance.chatRoom.OutputSystemMessage(message);
    }

    private static void RestartGame()
    {
        if (!isMasterClient)
        {
            instance.chatRoom.OutputErrorNotMasterClient();
            return;
        }

        var chatMessage = new object[] { FormatSystemMessage("MasterClient has restarted the game!"), string.Empty };
        instance.photonView.RPC(nameof(FengGameManagerMKII.Chat), PhotonTargets.All, chatMessage);
        instance.RestartRound();
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
            instance.photonView.RPC(nameof(FengGameManagerMKII.ChatPM), targetPlayer, new object[] { GetPlayerName(player), chatMessage.ToString() });

            var message = $"TO [{targetPlayer.ID}] {GetPlayerName(targetPlayer)}:{chatMessage}";
            instance.chatRoom.AddMessage(message);
        }
        else
        {
            instance.chatRoom.OutputErrorPlayerNotFound(playerIdString);
        }
    }

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
        if (!isMasterClient)
        {
            instance.chatRoom.OutputErrorNotMasterClient();
            return;
        }

        ChatCommand command;
        if (input.Count() >= 3)
        {
            if (Enum.TryParse(input[1], true, out command))
            {
                string parameter = input[2];
                int maxPlayers;
                float time;

                switch (command)
                {
                    case ChatCommand.Max:
                        if (int.TryParse(parameter, out maxPlayers))
                        {
                            ChangeRoomMaxPlayers(maxPlayers);
                        }
                        else
                        {
                            instance.chatRoom.OutputErrorMessage($"{parameter} is not a number.");
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
    private static void ChangeRoomMaxPlayers(int maxPlayers)
    {
        room.MaxPlayers = maxPlayers;
        var chatMessage = new object[] { FormatSystemMessage($"Max players changed to {maxPlayers}!"), string.Empty };
        instance.photonView.RPC(nameof(FengGameManagerMKII.Chat), PhotonTargets.All, chatMessage);
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

    private static void TogglePauseGame()
    {
        if (!isMasterClient)
        {
            instance.chatRoom.OutputErrorNotMasterClient();
            return;
        }


        if (PhotonNetwork.offlineMode)
        {
            instance.InGameUI.TogglePauseMenu();
        }
        else
        {
            Service.Pause.photonView.RPC(nameof(IPauseService.PauseRpc), PhotonTargets.All, !Service.Pause.IsPaused(), false);
            var chatMessage = Service.Pause.IsPaused() && !Service.Pause.IsUnpausing() ? "MasterClient has paused the game." : "MasterClient has unpaused the game.";
            instance.photonView.RPC(nameof(FengGameManagerMKII.Chat), PhotonTargets.All, new object[] { FormatSystemMessage(chatMessage), string.Empty });
        }
    }

    private static void TogglePreserveKdr(string parameter)
    {

        if (!isMasterClient)
        {
            instance.chatRoom.OutputErrorNotMasterClient();
            return;
        }

        string message;
        ChatCommand command;
        if (Enum.TryParse(parameter, true, out command))
        {
            switch (command)
            {
                case ChatCommand.Kdr:
                    if (!GameSettings.Gamemode.SaveKDROnDisconnect.Value)
                    {
                        GameSettings.Gamemode.SaveKDROnDisconnect = true;
                        message = "KDRs will be preserved from disconnects.";
                    }
                    else
                    {
                        GameSettings.Gamemode.SaveKDROnDisconnect = false;
                        message = "KDRs will not be preserved from disconnects.";
                    }

                    instance.chatRoom.AddMessage(message);
                    break;
            }
        }
    }

    private static void ResetKdAll()
    {
        if (!isMasterClient)
        {
            instance.chatRoom.OutputErrorNotMasterClient();
            return;
        }

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
        instance.photonView.RPC(nameof(FengGameManagerMKII.Chat), PhotonTargets.All, chatMessage);

    }

    private static void SwitchToTeam(string team)
    {
        ChatCommand teamEnum;
        string message = string.Empty;
        if (GameSettings.Gamemode.TeamMode == TeamMode.NoSort)
        {
            if (Enum.TryParse(team, true, out teamEnum))
            {
                switch (teamEnum)
                {
                    case ChatCommand.None:
                        SwitchTeam((int) teamEnum);
                        message = "You have joined individuals.";
                        break;
                    case ChatCommand.Cyan:
                        SwitchTeam((int) teamEnum);
                        message = FormatTextColorCyan("You have joined team cyan.");
                        break;
                    case ChatCommand.Magenta:
                        SwitchTeam((int) teamEnum);
                        message = FormatTextColorMagenta("You have joined team magenta.");
                        break;
                    default:
                        instance.chatRoom.OutputErrorMessage("Invalid team name. Accepted text values are none, cyan or magenta.");
                        break;
                }
                instance.chatRoom.AddMessage(message);

            }
            else
            {
                instance.chatRoom.OutputErrorMessage($"{team} is not a valid team.");
            }
        }
        else
        {
            instance.chatRoom.OutputErrorMessage("Teams are locked or disabled.");
        }
    }

    private static void ClearChatAll()
    {
        if (!isMasterClient)
        {
            instance.chatRoom.OutputErrorNotMasterClient();
            return;
        }

        instance.photonView.RPC(nameof(ClearChat), PhotonTargets.All);
    }

    private static void ClearChat()
    {
        instance.chatRoom.ClearMessages();
    }

    private static void SwitchTeam(int team)
    {
        instance.photonView.RPC(nameof(FengGameManagerMKII.setTeamRPC), player, new object[] { team });
        foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (gameObject.GetPhotonView().isMine)
            {
                gameObject.GetComponent<Hero>().MarkDie();
                gameObject.GetComponent<Hero>().photonView.RPC(nameof(Hero.NetDie2), PhotonTargets.All, new object[] { -1, "Team Switch" });
            }
        }
    }

    /// <summary>
    /// Teleports the player. MC only
    /// </summary>
    /// <param name="cords"></param>
    private static void Teleport(string[] parameters)
    {
        if (!PhotonNetwork.isMasterClient)
        {
            instance.chatRoom.UpdateChat("Only the MasterClient can teleport themselves");
            return;
        }

        if (parameters.Length != 4)
        {
            instance.chatRoom.UpdateChat("Invalid syntax. Correct syntax: /teleport X Y Z");
            return;
        }

        var player = Service.Player.Self;
        if (Service.Player.Self == null)
        {
            instance.chatRoom.UpdateChat("Cannot use teleport if the player hasn't spawned yet!");
            return;
        }

        if (float.TryParse(parameters[1], out var x)
            && float.TryParse(parameters[2], out var y)
            && float.TryParse(parameters[3], out var z))
        {
            player.transform.position = new Vector3(x, y, z);
        }
        else
        {
            instance.chatRoom.UpdateChat("Invalid parameters. Assure that all parameters are valid numbers!");
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

        if (!Enum.TryParse(commands[0], true, out command))
        {
            instance.chatRoom.OutputErrorMessage($"No command matches {commands[0]}");
            return;
        }
        if (commands.Count() > 1)
        {
            parameter = commands[1];
        }

        string message;
        switch (command)
        {
            case ChatCommand.MusicVolume:
                SetMusicVolume(parameter);
                break;
            case ChatCommand.Cloth:
                message = ClothFactory.GetDebugInfo();
                instance.chatRoom.AddMessage(message);
                break;
            case ChatCommand.Aso:
                TogglePreserveKdr(parameter);
                break;
            case ChatCommand.Pause:
                TogglePauseGame();
                break;
            case ChatCommand.Checklevel:
                CheckLevel(player);
                break;
            case ChatCommand.Isrc:
                OutputIsRc();
                break;
            case ChatCommand.Ignorelist:
                OutputIgnoreList();
                break;
            case ChatCommand.Room:
                ChangeRoomProperties(commands);
                break;
            case ChatCommand.Resetkd:
                ResetKd();
                break;
            case ChatCommand.Resetkdall:
                ResetKdAll();
                break;
            case ChatCommand.Pm:
                SendPrivateMessage(commands);
                break;
            case ChatCommand.Team:
                SwitchToTeam(parameter);
                break;
            case ChatCommand.Restart:
                RestartGame();
                break;
            case ChatCommand.Specmode:
                ToggleSpecMode();
                break;
            case ChatCommand.Fov:
                SetFov(parameter);
                break;
            case ChatCommand.Colliders:
                OutputCollisions();
                break;
            case ChatCommand.Spectate:
                SpectatePlayer(parameter);
                break;
            case ChatCommand.Revive:
                RevivePlayer(parameter);
                break;
            case ChatCommand.Reviveall:
                ReviveAllPlayers();
                break;
            case ChatCommand.Unban:
                UnbanPlayer(parameter);
                break;
            case ChatCommand.Rules:
                OutputRules();
                break;
            case ChatCommand.Kick:
                KickPlayer(parameter);
                break;
            case ChatCommand.Ban:
                BanPlayer(parameter);
                break;
            case ChatCommand.Banlist:
                OutputBanList();
                break;
            case ChatCommand.Clear:
                ClearChat();
                break;
            case ChatCommand.ClearAll:
                ClearChatAll();
                break;
            case ChatCommand.Teleport:
                Teleport(commands);
                break;
            default:
                break;
        }
    }

    private static void SetMusicVolume(string parameter)
    {
        if (float.TryParse(parameter, NumberStyles.Float, CultureInfo.InvariantCulture, out var volume))
        {
            Service.Music.SetMusicVolume(new MusicVolumeChangedEvent(volume));
        }     
    }
}
