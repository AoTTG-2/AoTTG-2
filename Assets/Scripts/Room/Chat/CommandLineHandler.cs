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

        public static string ErrorNotMasterClient()
        {
            return $"<color=#FFCC00>Error: Not Master Client.</color>";
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
            string result = string.Empty;
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
                var message = FormatSystemMessage("Error: You can't ban yourself.");
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
                    var message = $"Error: No player with ID #{playerToBan.ID} could be found.";
                    instance.chatRoom.AddMessage(message);
                }
            }
        }

        public static void KickPlayer(int playerId)
        {
            if (playerId == player.ID)
            {
                var message = "Error: You can't kick yourself.";
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
                    var chatMessage = new object[] { $"{GetPlayerName(playerToKick)} has been kicked from the server!", string.Empty };
                    instance.photonView.RPC("Chat", PhotonTargets.All, chatMessage);
                }

                if (!playerList.Any(p => p.ID == playerId))
                {
                    var message = FormatSystemMessage($"Error: No player with Id {playerId} found.");
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
                    var message = FormatSystemMessage($"Error: No player with Id {key} found");
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

        public static void SetFov(int fov)
        {
            Camera.main.fieldOfView = fov;
            var message = FormatSystemMessage($"Field of vision set to {fov}.");
            instance.chatRoom.AddMessage(message);
        }

        public static void EnterExitSpecMode()
        {
            if ((int)settings[0xf5] == 0)
            {
                settings[0xf5] = 1;
                instance.EnterSpecMode(true);
                var message = FormatSystemMessage("You have entered spectator mode.");
                instance.chatRoom.AddMessage(message);
            }
            else
            {
                settings[0xf5] = 0;
                instance.EnterSpecMode(false);
                var message = FormatSystemMessage("You have exited spectator mode.");
                instance.chatRoom.AddMessage(message);
            }
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

        public static void SendPrivateMessage(string input)
        {
            var inputPlayerName = input.Split(new char[] { ' ' });
            PhotonPlayer targetPlayer = PhotonPlayer.Find(Convert.ToInt32(inputPlayerName[1]));

            string whatIsThisVar = string.Empty;
            for (var nameIndex = 2; nameIndex < inputPlayerName.Length; nameIndex++)
            {
                whatIsThisVar = inputPlayerName[nameIndex] + " ";
            }
            instance.photonView.RPC("ChatPM", targetPlayer, new object[] { SetNameColorDependingOnteam(player), whatIsThisVar });

            var message = $"TO [{targetPlayer.ID}] {SetNameColorDependingOnteam(targetPlayer)}:{whatIsThisVar}";
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

        public static void ChangeRoomProperties(string input)
        {
            if (isMasterClient)
            {
                if (input.Substring(6).StartsWith("max"))
                {
                    var maxPlayers = Convert.ToInt32(input.Substring(10));
                    instance.maxPlayers = maxPlayers;
                    room.MaxPlayers = maxPlayers;
                    var chatMessage = new object[] { FormatSystemMessage($"Max players changed to {input.Substring(10)}!"), string.Empty };
                    instance.photonView.RPC("Chat", PhotonTargets.All, chatMessage);
                }
                else if (input.Substring(6).StartsWith("time"))
                {
                    instance.addTime(Convert.ToSingle(input.Substring(11)));
                    var chatMessage = new object[] { FormatSystemMessage($"{input.Substring(11)} seconds added to the clock."), string.Empty };
                    instance.photonView.RPC("Chat", PhotonTargets.All, chatMessage);
                }
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
            if (masterRC)
            {
                var message = "is RC";
                instance.chatRoom.AddMessage(message);
            }
            else
            {
                var message = "not RC";
                instance.chatRoom.AddMessage(message);
            }
        }

        public static void CheckLevel()
        {
            foreach (PhotonPlayer player in playerList)
            {
                var message = RCextensions.returnStringFromObject(player.CustomProperties[PhotonPlayerProperty.currentLevel]);
                instance.chatRoom.AddMessage(message);
            }
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

        public static void PreserveKdrOnOFF(string input)
        {
            if (isMasterClient)
            {
                switch (input.Substring(5))
                {
                    // Info: 
                    case "kdr":
                        if (!FengGameManagerMKII.Gamemode.Settings.SaveKDROnDisconnect)
                        {
                            FengGameManagerMKII.Gamemode.Settings.SaveKDROnDisconnect = true;
                            var message = "KDRs will be preserved from disconnects.";
                            instance.chatRoom.AddMessage(message);
                        }
                        else
                        {
                            FengGameManagerMKII.Gamemode.Settings.SaveKDROnDisconnect = false;
                            var message = "KDRs will not be preserved from disconnects.";
                            instance.chatRoom.AddMessage(message);
                        }
                        break;
                }
            }
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
            if (FengGameManagerMKII.Gamemode.Settings.TeamMode == TeamMode.NoSort)
            {
                if (team == "1" || team == "cyan")
                {
                    instance.photonView.RPC("setTeamRPC", player, new object[] { 1 });
                    var message = FormatTextColor00FFFF("You have joined team cyan.");
                    instance.chatRoom.AddMessage(message);
                    foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("Player"))
                    {
                        if (gameObject.GetPhotonView().isMine)
                        {
                            gameObject.GetComponent<Hero>().markDie();
                            gameObject.GetComponent<Hero>().photonView.RPC("netDie2", PhotonTargets.All, new object[] { -1, "Team Switch" });
                        }
                    }
                }
                else if (team == "2" || team == "magenta")
                {
                    instance.photonView.RPC("setTeamRPC", player, new object[] { 2 });
                    var message = FormatTextColorFF00FF("You have joined team magenta.");
                    instance.chatRoom.AddMessage(message);
                    foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("Player"))
                    {
                        if (gameObject.GetPhotonView().isMine)
                        {
                            gameObject.GetComponent<Hero>().markDie();
                            gameObject.GetComponent<Hero>().photonView.RPC("netDie2", PhotonTargets.All, new object[] { -1, "Team Switch" });
                        }
                    }
                }
                else if (team == "0" || team == "individual")
                {
                    instance.photonView.RPC("setTeamRPC", player, new object[] { 0 });
                    var message = "You have joined individuals.";
                    instance.chatRoom.AddMessage(message);
                    foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("Player"))
                    {
                        if (gameObject.GetPhotonView().isMine)
                        {
                            gameObject.GetComponent<Hero>().markDie();
                            gameObject.GetComponent<Hero>().photonView.RPC("netDie2", PhotonTargets.All, new object[] { -1, "Team Switch" });
                        }
                    }
                }
                else
                {
                    var message = FormatSystemMessage("Error: Invalid team code. Accepted values are 0, 1 and 2.");
                    instance.chatRoom.AddMessage(message);
                }
            }
            else
            {
                var message = FormatSystemMessage("Error: Teams are locked or disabled.");
                instance.chatRoom.AddMessage(message);
            }
        }
    }
}
