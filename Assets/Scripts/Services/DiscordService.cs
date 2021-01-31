using Assets.Scripts.Room;
using Assets.Scripts.Services.Interface;
using Discord;
using Photon;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Services
{
    public class DiscordService : PunBehaviour, IDiscordService
    {
        private Discord.Discord discord;
        private ActivityManager activityManager;

        private ActivityAssets assetsStruct;
        private Coroutine joiningRoutine;
        private bool isJoinedLobby;

        private const int Timeout = 3;
        private const string LargeImageKey = "logo";
        private const string LargeText = "AoTTG2";
        private const long AppID = 764974724907270194;


        private void Awake()
        {
            discord = new Discord.Discord(AppID, (ulong) CreateFlags.NoRequireDiscord);
            activityManager = discord.GetActivityManager();

            activityManager.OnActivityJoin += JoinViaDiscord;
            SceneManager.activeSceneChanged += OnSceneChanged;
            
            activityManager.OnActivityJoinRequest += (ref User user) =>
            {
                FengGameManagerMKII.instance.chatRoom.OutputSystemMessage($"{user.Username}#{user.Discriminator} has requested to join the game");    //Refactor, when ChatService implemented.
            };


            assetsStruct = new ActivityAssets
            {
                LargeImage = LargeImageKey,
                LargeText = LargeText
            };

            activityManager.RegisterCommand(GetApplicationPath());
        }

        private void Update()
        {
            discord.RunCallbacks();
        }
        
        private void OnDestroy()
        {
            discord.Dispose();
        }

        private void OnSceneChanged(Scene oldScene, Scene newScene)
        {
            if (newScene.buildIndex == 0)
            {
                InMenu();
            }
        }

        private void JoinViaDiscord(string roomID)
        {
            Service.Photon.Connect();
            if (joiningRoutine != null)
                StopCoroutine(joiningRoutine);

            joiningRoutine = StartCoroutine(JoinRoutine(roomID));
        }

        private IEnumerator JoinRoutine(string roomID)
        {
            float startTime = Time.time;
            Service.Photon.Connect();

            while (!isJoinedLobby)
            {
                yield return new WaitForSeconds(0.2f);
                if (Time.time - startTime > Timeout)
                {
                    break;
                }
            }

            if (isJoinedLobby)
                PhotonNetwork.JoinRoom(roomID);
        }

        public override void OnJoinedLobby()
        {
            base.OnJoinedLobby();
            isJoinedLobby = true;
        }

        public override void OnDisconnectedFromPhoton()
        {
            base.OnDisconnectedFromPhoton();
            isJoinedLobby = false;
        }

        public void UpdateDiscordActivity(global::Room room)
        {
            if (room.GetName().Equals("Singleplayer"))
                UpdateSinglePlayerActivity(room);
            else
                UpdateMultiPlayerActivity(room);
        }

        private void InMenu()
        {
            var activityStruct = new Activity
            {
                Assets = assetsStruct,
                State = "In Menu"
            };
            activityManager.UpdateActivity(activityStruct, (result) =>
            {
                Debug.Log(result == Result.Ok
                    ? $"Updated to Main Menu"
                    : $"Failed to Update to Main Menu");
            });
        }

        private void UpdateSinglePlayerActivity(global::Room room)
        {
            var activityStruct = new Activity
            {
                Assets = assetsStruct,
                State = "SinglePlayer",
                Details = room.GetLevel().Name + " - " + room.GetGamemode(),
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
            var activityStruct = new Activity
            {
                Assets = assetsStruct,
                State = room.GetName() + "- [" + PhotonNetwork.CloudRegion.ToString().ToUpper() + "]",
                Details = room.GetLevel().Name + " - " + room.GetGamemode(),
                Party = new ActivityParty
                {
                    Size = new PartySize
                    {
                        CurrentSize = room.PlayerCount,
                        MaxSize = room.MaxPlayers
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

        #region Helper Methods

        private static string GetApplicationPath()
        {
            var path = Application.dataPath;
            switch (Application.platform)
            {
                case RuntimePlatform.WindowsPlayer:
                    path = path.Replace("_Data", string.Empty);
                    path += ".exe";
                    break;
                case RuntimePlatform.OSXPlayer:
                    path = Directory.GetParent(path).ToString();
                    path += ".app";
                    break;
                default:
                    Debug.Log($"DiscordRPC: {Application.platform} is not supported");
                    break;
            }

            return path;
        }

        #endregion
    }
}