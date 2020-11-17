using System;
using Assets.Scripts.Room;
using Assets.Scripts.Services.Interface;
using Discord;
using Photon;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Services
{
    public class DiscordService : PunBehaviour, IDiscordService
    {
        private Discord.Discord discord;
        private ActivityManager activityManager;

        private Activity activityStruct;
        private ActivityAssets assetsStruct;

        private const string LargeImageKey = "aottg_2_title";
        private const string LargeText = "AoTTG2";
        private const long AppID = 730150236185690172;


        private void Awake()
        {
            discord = new Discord.Discord(AppID, (UInt64) Discord.CreateFlags.Default);
            activityManager = discord.GetActivityManager();
            activityManager.OnActivityJoin += JoinViaDiscord;
            SceneManager.activeSceneChanged += OnSceneChanged;

            assetsStruct = new ActivityAssets
            {
                LargeImage = LargeImageKey,
                LargeText = LargeText
            };
        }

        private void Start()
        {
            activityManager.RegisterCommand(GetApplicationPath());
            activityStruct = new Activity
            {
                Assets = assetsStruct
            };
            InMenu();
        }

        private void Update()
        {
            discord.RunCallbacks();
        }

        private void OnApplicationQuit()
        {
            CloseSocket();
        }

        private void OnSceneChanged(Scene oldScene, Scene newScene)
        {
            if (newScene.buildIndex == 0 && oldScene.buildIndex > 0)
            {
                InMenu();
            }
        }

        private void InMenu()
        {
            activityStruct.State = "In Menu";
            activityStruct.Details = "";
            activityManager.UpdateActivity(activityStruct, (result) =>
            {
                Debug.Log(result == Result.Ok
                    ? $"Updated to Main Menu"
                    : $"Failed to Update to Main Menu");
            });
        }


        #region Service Methods

        public void UpdateDiscordActivity(global::Room room)
        {
            if (room.GetName().Equals("Singleplayer"))
                UpdateSinglePlayerActivity(room);
            else
                UpdateMultiPlayerActivity(room);
        }

        private void UpdateSinglePlayerActivity(global::Room room)
        {
            Debug.Log($"Room name = {room.GetName()}, room level = {room.GetLevel()}");
            activityStruct = new Activity
            {
                Assets = assetsStruct,
                State = "SinglePlayer",
                Details = room.GetLevel() + " - " + room.GetGamemode(),
            };
            activityManager.UpdateActivity(activityStruct,
                (result) =>
                {
                    Debug.Log(result == Result.Ok
                        ? $"Updated Single player presence."
                        : $"Failed to Update Single player presence.");
                });
        }

        private void UpdateMultiPlayerActivity(global::Room room)
        {
            activityStruct = new Activity
            {
                Assets = assetsStruct,
                State = room.GetName() + "- [" + PhotonNetwork.CloudRegion.ToString().ToUpper() + "]",
                Details = room.GetLevel() + " - " + room.GetGamemode(),
                Party = new ActivityParty
                {
                    Size = new PartySize
                    {
                        CurrentSize = room.PlayerCount,
                        MaxSize = room.MaxPlayers >= room.PlayerCount ? room.MaxPlayers : 10
                    },
                    Id = room.GetHashCode().ToString(),
                },
                Secrets = new ActivitySecrets
                {
                    Join = room.Name,
                }
            };

            activityManager.UpdateActivity(activityStruct,
                (result) =>
                {
                    Debug.Log(result == Result.Ok
                        ? $"Updated Multi-player party."
                        : $"Failed to Update Multi-player party.");
                });
        }

        public Discord.Discord GetSocket()
        {
            return discord;
        }

        public void CloseSocket()
        {
            discord.Dispose();
        }

        public void JoinViaDiscord(string roomID)
        {
            PhotonNetwork.JoinRoom(roomID);
        }

        #endregion

        #region Helper Methods

        private static string GetApplicationPath()
        {
            string path = System.Environment.CurrentDirectory;
            path += "\\";
            if (Application.platform == RuntimePlatform.WindowsPlayer)
                path += "AoTTG 2.exe";
            else if (Application.platform == RuntimePlatform.OSXPlayer)
                path += "AoTTG 2.app";
            else
                Debug.Log($"Unrecognized Platform");

            return path;
        }

        #endregion
    }
}