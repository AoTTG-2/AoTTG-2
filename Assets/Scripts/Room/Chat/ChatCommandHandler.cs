using Assets.Scripts.Characters.Titan;
using Assets.Scripts.Gamemode.Options;
using ExitGames.Client.Photon;
using System;
using UnityEngine;
using static PhotonNetwork;
using static FengGameManagerMKII;
using System.Linq;

namespace Assets.Scripts.Room.Chat
{
    public static class ChatCommandHandler
    {
        /// <summary>
        /// Formats text as <color=#00FFFF>{input}</color>
        /// </summary>
        /// <param name="input"></param>
        /// <returns><color=#00FFFF>{input}</color></returns>
        public static string FormatTextColor00FFFF(string input)
        {
            return $"<color=#00FFFF>{input}</color>";
        }

        /// <summary>
        /// Formats text as <color=#FF00FF>{input}</color>
        /// </summary>
        /// <param name="input"></param>
        /// <returns><color=#FF00FF>{input}</color></returns>
        public static string FormatTextColorFF00FF(string input)
        {
            return $"<color=#FF00FF>{input}</color>";
        }

        /// <summary>
        /// Formats text as <color=#FFCC00>{input}</color>
        /// </summary>
        /// <param name="input"></param>
        /// <returns><color=#FFCC00>{input}</color></returns>
        public static string FormatSystemMessage(string input)
        {
            return $"<color=#FFCC00>{input}</color>";
        }

        public static void OutputSystemMessage(string input)
        {
            var message = $"<color=#FFCC00>{input}</color>"; ;
            instance.chatRoom.AddMessage(message);
        }

        /// <summary>
        /// Formats text as <color=#FF0000>Error: {input}</color>
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static void OutputErrorMessage(string input)
        {
            var message = $"<color=#FF0000>Error: {input}</color>";
            instance.chatRoom.AddMessage(message);
        }

        /// <summary>
        /// Outputs Not Master Client Error to chat
        /// </summary>
        public static void OutputErrorNotMasterClient()
        {
            OutputErrorMessage("Not Master Client");
        }

        /// <summary>
        /// Outputs Flayer Not Found Error to chat
        /// </summary>
        /// <param name="playerId"></param>
        private static void OutputErrorPlayerNotFound(string playerId)
        {
            OutputErrorMessage($"No player with ID #{playerId} could be found.");
        }

        /// <summary>
        /// Gets the name of PhotonPlayer.player
        /// </summary>
        /// <param name="player"></param>
        /// <returns>PhotonPlayerProperty.name</returns>
        public static string GetPlayerName(PhotonPlayer player)
        {
            return RCextensions.returnStringFromObject(player.CustomProperties[PhotonPlayerProperty.name]);
        }

        /// <summary>
        /// sets color of name in chat depending on what team PhotonPlayer.player is in
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public static string SetNameColorDependingOnteam(PhotonPlayer player)
        {
            var name = GetPlayerName(player);
            var playerTeam = player.GetTeam();
            switch (playerTeam)
            {
                case PunTeams.Team.red:
                    name = FormatTextColor00FFFF(name);
                    break;
                case PunTeams.Team.blue:
                    name = FormatTextColorFF00FF(name);
                    break;
                default:
                    break;
            }

            return name;
        }

        /// <summary>
        /// Outputs list of banned players to chat
        /// </summary>
        private static void OutputBanList()
        {
            var message = FormatSystemMessage("List of banned players:");
            instance.chatRoom.AddMessage(message);
            foreach (int key in banHash.Keys)
            {
                message = FormatSystemMessage($"{key}:{banHash[key]}");
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
                    OutputErrorMessage("You can't ban yourself.");
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
                        OutputErrorPlayerNotFound(playerIdString);
                        return;
                    }

                    if (OnPrivateServer || isMasterClient)
                    {
                        instance.kickPlayerRC(playerToBan, true, string.Empty);
                        var chatMessage = new object[] { FormatSystemMessage($"{GetPlayerName(playerToBan)} has been banned from the server!"), string.Empty };
                        instance.photonView.RPC("Chat", PhotonTargets.All, chatMessage);
                    }
                }
            }
            else
            {
                OutputErrorPlayerNotFound(playerIdString);
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
                    OutputErrorMessage("You can't kick yourself.");
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
                        OutputErrorPlayerNotFound(playerIdString);
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
                OutputErrorPlayerNotFound(playerIdString);
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
                        OutputErrorPlayerNotFound(key.ToString());
                    }
                }
                else
                {
                    OutputErrorPlayerNotFound(playerId);
                }
            }
            else
            {
                OutputErrorNotMasterClient();
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
                    OutputErrorPlayerNotFound(playerIdString);
                }
            }
            else
            {
                OutputErrorNotMasterClient();
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
                OutputErrorPlayerNotFound(playerIdString);
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
                OutputSystemMessage($"Field of vision set to {fov}.");
            }
            else
            {
               OutputErrorMessage("Fov has to be a number");
            }
        }

        /// <summary>
        /// Enter/Exit spectate mode
        /// </summary>
        private static void EnterExitSpecMode()
        {
            string message;
            if ((int)settings[0xf5] == 0)
            {
                settings[0xf5] = 1;
                instance.EnterSpecMode(true);
                message = FormatSystemMessage("You have entered spectator mode.");
            }
            else
            {
                settings[0xf5] = 0;
                instance.EnterSpecMode(false);
                message = FormatSystemMessage("You have exited spectator mode.");
            }

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
                OutputErrorNotMasterClient();
            }
        }

        /// <summary>
        /// Send private message to player with Id
        /// </summary>
        /// <param name="input"></param>
        private static void SendPrivateMessage(string[] input)
        {
            int playerId;
            var playerIdString = input[1];
            if (int.TryParse(playerIdString, out playerId))
            {
                var targetPlayer = PhotonPlayer.Find(playerId);

                var whatIsThisVar = string.Empty;
                for (var nameIndex = 2; nameIndex < input.Length; nameIndex++)
                {
                    whatIsThisVar += input[nameIndex] + " ";
                }
                instance.photonView.RPC("ChatPM", targetPlayer, new object[] { SetNameColorDependingOnteam(player), whatIsThisVar });

                var message = $"TO [{targetPlayer.ID}] {SetNameColorDependingOnteam(targetPlayer)}:{whatIsThisVar}";
                instance.chatRoom.AddMessage(message);
            }
            else
            {
                OutputErrorPlayerNotFound(playerIdString);
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

        /// <summary>
        /// Set max players in room or add time to game
        /// </summary>
        /// <param name="input"></param>
        private static void ChangeRoomProperties(string[] input)
        {
            string command = input[1].ToLower();
            string parameter = input[2];
            var chatMessage = new object();
            if (isMasterClient)
            {
                if (command == "max")
                {
                    int maxPlayers;
                    if (int.TryParse(parameter, out maxPlayers))
                    {
                        instance.maxPlayers = maxPlayers;
                        room.MaxPlayers = maxPlayers;
                        chatMessage = new object[] { FormatSystemMessage($"Max players changed to {maxPlayers}!"), string.Empty };
                        instance.photonView.RPC("Chat", PhotonTargets.All, chatMessage);
                    }
                    else
                    {
                        OutputErrorMessage($"{parameter} is not a number.");
                    }
                    
                }
                else if (command == "time")
                {
                    float time;
                    if (float.TryParse(parameter, out time))
                    {
                        instance.addTime(time);
                        chatMessage = new object[] { FormatSystemMessage($"{input[2]} seconds added to the clock."), string.Empty };
                    }
                    else
                    {
                        OutputErrorMessage("Time to add must be a number.");
                    }
                    
                }
                else
                {
                   OutputErrorMessage("Valid room attributes are max or time");
                }

                instance.photonView.RPC("Chat", PhotonTargets.All, chatMessage);
            }
            else
            {
                OutputErrorNotMasterClient();
            }
        }

        /// <summary>
        /// Output ignore list to chat
        /// </summary>
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
            string message;
            if (masterRC)
            {
                message = "Is RC";
            }
            else
            {
                message = "Not RC";
                
            }

            instance.chatRoom.AddMessage(message);
        }

        /// <summary>
        /// Outputs name of current level to chat
        /// </summary>
        /// <param name="player"></param>
        private static void CheckLevel(PhotonPlayer player)
        {
            var message = RCextensions.returnStringFromObject(player.CustomProperties[PhotonPlayerProperty.currentLevel]);
            instance.chatRoom.AddMessage(message);
        }
        
        /// <summary>
        /// Unpause currently paused game
        /// </summary>
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
                OutputErrorNotMasterClient();
            }
        }

        /// <summary>
        /// Pause the current game
        /// </summary>
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
                OutputErrorNotMasterClient();
            }
        }

        /// <summary>
        /// turn on/off preserve KDR on diconnect
        /// </summary>
        /// <param name="command"></param>
        private static void PreserveKdrOnOFF(string command)
        {
            string message;
            if (isMasterClient)
            {
                switch (command)
                {
                    // Info: 
                    case "kdr":
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
            else
            {
                OutputErrorNotMasterClient();
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
                OutputErrorNotMasterClient();
            }
        }

        /// <summary>
        /// Switch to team
        /// </summary>
        /// <param name="team"></param>
        private static void SwitchToTeam(string team)
        {
            int teamInt;
            string message = string.Empty;
            if (FengGameManagerMKII.Gamemode.Settings.TeamMode == TeamMode.NoSort)
            {
                if (int.TryParse(team, out teamInt))
                {
                    switch (teamInt)
                    {
                        case 0:
                            SwitchTeam(0);
                            message = "You have joined individuals.";
                            break;
                        case 1:
                            SwitchTeam(1);
                            message = FormatTextColor00FFFF("You have joined team cyan.");
                            break;
                        case 2:
                            SwitchTeam(2);
                            message = FormatTextColorFF00FF("You have joined team magenta.");
                            break;
                        default:
                            OutputErrorMessage("Invalid team code. Accepted number values are 0, 1 or 2.");
                            break;
                    }
                }
                else
                {
                    switch (team.ToLower())
                    {
                        case "cyan":
                            SwitchTeam(1);
                            message = FormatTextColor00FFFF("You have joined team cyan.");
                            break;
                        case "magenta":
                            SwitchTeam(2);
                            message = FormatTextColorFF00FF("You have joined team magenta.");
                            break;
                        default:
                            OutputErrorMessage("Invalid team name. Accepted text values are cyan or magenta.");
                            break;
                    }
                }

                instance.chatRoom.AddMessage(message);
            }
            else
            {
                OutputErrorMessage("Teams are locked or disabled.");
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
                OutputErrorNotMasterClient();
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
            var commands = chatCommand.Replace("/", "").ToLower().Split(' ');
            var parameter = commands[1];
            string message;
            switch (commands[0])
            {
                // Info: 
                case "cloth":
                    message = ClothFactory.GetDebugInfo();
                    instance.chatRoom.AddMessage(message);
                    break;
                // Info: 
                case "aso":
                    PreserveKdrOnOFF(parameter);
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
                    ChangeRoomProperties(commands);
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
                    SendPrivateMessage(commands);
                    break;
                // Info: Switch team
                case "team":
                    var team = parameter;
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
                    var fov = parameter;
                    SetFov(fov);
                    break;
                // Info: 
                case "colliders":
                    OutputCollisions();
                    break;
                // Info: 
                case "spectate":
                    SpectatePlayer(parameter);
                    break;
                // Info: 
                case "kill":
                    break;
                // Info: 
                case "revive":
                    RevievePlayer(parameter);
                    break;
                // Info: 
                case "reviveall":
                    ReviveAllPlayers();
                    break;
                // Info: 
                case "unban":
                    UnbanPlayer(parameter);
                    break;
                // Info: 
                case "rules":
                    OutputRules();
                    break;
                // Info: 
                case "kick":
                    KickPlayer(parameter);
                    break;
                // Info: 
                case "ban":
                    BanPlayer(parameter);
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
}
