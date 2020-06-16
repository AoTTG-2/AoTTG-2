using Assets.Scripts.Characters.Titan;
using Assets.Scripts.Gamemode.Options;
using ExitGames.Client.Photon;
using System;
using UnityEngine;
using static PhotonNetwork;
using static FengGameManagerMKII;
using WebSocketSharp;
using System.Linq;

namespace Assets.Scripts.Room.Chat
{
    public static class CommandLineHandler
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

        /// <summary>
        /// Formats text as <color=#FF0000>Error: {input}</color>
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string FormatErrorMessage(string input)
        {
            return $"<color=#FF0000>Error: {input}</color>";
        }

        public static string ErrorNotMasterClient()
        {
            return FormatErrorMessage("Not Master Client");
        }

        private static string ErrorPlayerNotFound(int playerId)
        {
            return FormatErrorMessage($"No player with ID #{playerId} could be found.");
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

        public static int GetPlayerTeam(PhotonPlayer player)
        {
            return RCextensions.returnIntFromObject(player.CustomProperties[PhotonPlayerProperty.RCteam]);
        }

        /// <summary>
        /// sets color of name in chat depending on what team PhotonPlayer.player is in
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public static string SetNameColorDependingOnteam(PhotonPlayer player)
        {
            var name = GetPlayerName(player).hexColor();
            if (name.IsNullOrEmpty())
            {
                var playerTeam = GetPlayerTeam(player);
                if (playerTeam != 0)
                {

                    if (playerTeam == 1)
                    {
                        name = FormatTextColor00FFFF(name);
                    }
                    else if (playerTeam == 2)
                    {
                        name = FormatTextColorFF00FF(name);
                    }
                }
            }

            return name;
        }

        public static void OutputBanList()
        {
            var message = FormatSystemMessage("List of banned players:");
            instance.chatRoom.AddMessage(message);
            foreach (int key in banHash.Keys)
            {
                message = FormatSystemMessage($"{key}:{banHash[key]}");
                instance.chatRoom.AddMessage(message);
            }
        }

        public static void BanPlayer(int playerId)
        {
            if (playerId == player.ID)
            {
                var message = FormatErrorMessage("You can't ban yourself.");
                instance.chatRoom.AddMessage(message);
            }
            else if (!(OnPrivateServer || isMasterClient))
            {
                var chatMessage = new object[] { $"/ban #{playerId}", LoginFengKAI.player.name };
                instance.photonView.RPC("Chat", PhotonTargets.All, chatMessage);
            }
            else
            {
                var playerToBan = playerList.FirstOrDefault(p => p.ID == playerId);

                if (OnPrivateServer || isMasterClient)
                {
                    instance.kickPlayerRC(playerToBan, true, string.Empty);
                    var chatMessage = new object[] { FormatSystemMessage($"{GetPlayerName(playerToBan)} has been banned from the server!"), string.Empty };
                    instance.photonView.RPC("Chat", PhotonTargets.All, chatMessage);
                }
                if (!playerList.Any(p => p.ID == playerId))
                {
                    var message = ErrorPlayerNotFound(playerId);
                    instance.chatRoom.AddMessage(message);
                }
            }
        }

        public static void KickPlayer(int playerId)
        {
            if (playerId == player.ID)
            {
                var message = FormatErrorMessage("You can't kick yourself.");
                instance.chatRoom.AddMessage(message);
            }
            else if (!(OnPrivateServer || isMasterClient))
            {
                var chatMessage = new object[] { $"/kick #{playerId}", LoginFengKAI.player.name };
                instance.photonView.RPC("Chat", PhotonTargets.All, chatMessage);
            }
            else
            {
                var playerToKick = playerList.FirstOrDefault(p => p.ID == playerId);

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

                if (!playerList.Any(p => p.ID == playerId))
                {
                    var message = ErrorPlayerNotFound(playerId);
                    instance.chatRoom.AddMessage(message);
                }
            }
        }

        public static void OutputRules()
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

        public static void UnbanPlayer(string input)
        {
            if (OnPrivateServer)
            {
                ServerRequestUnban(input.Substring(7));
            }
            else if (isMasterClient)
            {
                var key = Convert.ToInt32(input.Substring(7));
                if (banHash.ContainsKey(key))
                {
                    var chatMessage = new object[] { $"{banHash[key]} has been unbanned from the server.", string.Empty };
                    instance.photonView.RPC("Chat", PhotonTargets.All, chatMessage);
                    banHash.Remove(key);
                }
                else
                {
                    var message = ErrorPlayerNotFound(key);
                    instance.chatRoom.AddMessage(message);
                }
            }
            else
            {
                var message = ErrorNotMasterClient();
                instance.chatRoom.AddMessage(message);
            }
        }

        public static void ReviveAllPlayers()
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

        public static void RevievePlayer(int playerId)
        {
            if (isMasterClient)
            {
                var player = playerList.FirstOrDefault(p => p.ID == playerId);

                if (playerList.Any(p => p.ID == playerId))
                {
                    var message = FormatSystemMessage($"Player {playerId} has been revived.");
                    instance.chatRoom.AddMessage(message);
                    instance.photonView.RPC("RespawnRpc", player);
                }
            }
        }

        public static void SpectatePlayer(int playerId)
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

        public static void OutputCollisions()
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

        public static void SetFov(string input)
        {
            int fov;
            string message;
            if (int.TryParse(input, out fov))
            {
                Camera.main.fieldOfView = fov;
                message = FormatSystemMessage($"Field of vision set to {fov}.");
            }
            else
            {
                message = FormatErrorMessage("Fov has to be a number");
            }

            instance.chatRoom.AddMessage(message);
        }

        public static void EnterExitSpecMode()
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

        public static void RestartGame()
        {
            if (isMasterClient)
            {
                var chatMessage = new object[] { FormatSystemMessage("MasterClient has restarted the game!"), string.Empty };
                instance.photonView.RPC("Chat", PhotonTargets.All, chatMessage);
                instance.restartRC();
            }
            else
            {
                var message = ErrorNotMasterClient();
                instance.chatRoom.AddMessage(message);
            }
        }

        public static void SendPrivateMessage(string[] input)
        {
            int playerId;
            string message;
            if (int.TryParse(input[1], out playerId))
            {
                var targetPlayer = PhotonPlayer.Find(playerId);

                var whatIsThisVar = string.Empty;
                for (var nameIndex = 2; nameIndex < input.Length; nameIndex++)
                {
                    whatIsThisVar += input[nameIndex] + " ";
                }
                instance.photonView.RPC("ChatPM", targetPlayer, new object[] { SetNameColorDependingOnteam(player), whatIsThisVar });

                message = $"TO [{targetPlayer.ID}] {SetNameColorDependingOnteam(targetPlayer)}:{whatIsThisVar}";
            }
            else
            {
                message = ErrorPlayerNotFound(playerId);
            }

            instance.chatRoom.AddMessage(message);
        }

        public static void ResetKd()
        {
            var hashTable = new Hashtable();
            hashTable.Add(PhotonPlayerProperty.kills, 0);
            hashTable.Add(PhotonPlayerProperty.deaths, 0);
            hashTable.Add(PhotonPlayerProperty.max_dmg, 0);
            hashTable.Add(PhotonPlayerProperty.total_dmg, 0);
            player.SetCustomProperties(hashTable);

            var message = FormatSystemMessage("Your stats have been reset.");
            instance.chatRoom.AddMessage(message);
        }

        public static void ChangeRoomProperties(string[] input)
        {
            var chatMessage = new object();
            if (isMasterClient)
            {
                if (input[1] == "max")
                {
                    var maxPlayers = Convert.ToInt32(input[1]);
                    instance.maxPlayers = maxPlayers;
                    room.MaxPlayers = maxPlayers;
                    chatMessage = new object[] { FormatSystemMessage($"Max players changed to {input[2]}!"), string.Empty };
                    instance.photonView.RPC("Chat", PhotonTargets.All, chatMessage);
                }
                else if (input[1] == "time")
                {
                    instance.addTime(Convert.ToSingle(input[2]));
                    chatMessage = new object[] { FormatSystemMessage($"{input[2]} seconds added to the clock."), string.Empty };
                    
                }
                else
                {
                    var message = FormatErrorMessage("Valid room attributes are (max, time)");
                    instance.chatRoom.AddMessage(message);
                }

                instance.photonView.RPC("Chat", PhotonTargets.All, chatMessage);
            }
            else
            {
                var message = ErrorNotMasterClient();
                instance.chatRoom.AddMessage(message);
            }
        }

        public static void OutputIgnoreList()
        {
            foreach (var ignoredPlayer in ignoreList)
            {
                var message = $"{ignoredPlayer}";
                instance.chatRoom.AddMessage(message);
            }
        }

        public static void OutputIsRc()
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

        public static void CheckLevel(PhotonPlayer player)
        {
            var message = RCextensions.returnStringFromObject(player.CustomProperties[PhotonPlayerProperty.currentLevel]);
            instance.chatRoom.AddMessage(message);
        }

        public static void UnPauseGame()
        {
            if (isMasterClient)
            {
                instance.photonView.RPC("pauseRPC", PhotonTargets.All, new object[] { false });
                var chatMessage = new object[] { FormatSystemMessage("MasterClient has unpaused the game."), string.Empty };
                instance.photonView.RPC("Chat", PhotonTargets.All, chatMessage);
            }
            else
            {
                var message = ErrorNotMasterClient();
                instance.chatRoom.AddMessage(message);
            }
        }

        public static void PauseGame()
        {
            if (isMasterClient)
            {
                instance.photonView.RPC("pauseRPC", PhotonTargets.All, new object[] { true });
                var chatMessage = new object[] { FormatSystemMessage("MasterClient has paused the game."), string.Empty };
                instance.photonView.RPC("Chat", PhotonTargets.All, chatMessage);
            }
            else
            {
                var message = ErrorNotMasterClient();
                instance.chatRoom.AddMessage(message);
            }
        }

        public static void PreserveKdrOnOFF(string[] input)
        {
            string message = string.Empty;
            if (isMasterClient)
            {
                switch (input[1])
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
                        break;
                }
            }
            else
            {
                message = ErrorNotMasterClient();
            }

            instance.chatRoom.AddMessage(message);
        }

        public static void ResetKdAll()
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
                var message = ErrorNotMasterClient();
                instance.chatRoom.AddMessage(message);
            }
        }

        public static void SwitchToTeam(string team)
        {
            string message;
            if (FengGameManagerMKII.Gamemode.Settings.TeamMode == TeamMode.NoSort)
            {
                if (team == "1" || team == "cyan")
                {
                    SwitchTeam(1);
                    message = FormatTextColor00FFFF("You have joined team cyan.");
                }
                else if (team == "2" || team == "magenta")
                {
                    SwitchTeam(2);
                    message = FormatTextColorFF00FF("You have joined team magenta.");
                }
                else if (team == "0" || team == "individual")
                {
                    SwitchTeam(0);
                    message = "You have joined individuals.";
                }
                else
                {
                    message = FormatErrorMessage("Invalid team code. Accepted values are 0, 1 and 2.");
                }
            }
            else
            {
                message = FormatErrorMessage("Teams are locked or disabled.");
            }

            instance.chatRoom.AddMessage(message);
        }

        public static void ClearChat()
        {
            if (isMasterClient)
            {
                instance.chatRoom.ClearMessages();
            }
            else
            {
                var message = ErrorNotMasterClient();
                instance.chatRoom.AddMessage(message);
            }
        }

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
    }
}
