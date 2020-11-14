using System;
using Assets.Scripts.Room;
using Discord;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DiscordRichPresence : MonoBehaviour
{
    private Activity activityStruct;

    private void Awake()
    {
        DiscordSocket.GetActivityManager().RegisterCommand(GetApplicationPath()); //Register application launch cmd
        DiscordSocket.GetActivityManager().OnActivityJoin += JoinViaDiscordInvite; // ActivityJoin callback
        SceneManager.activeSceneChanged += OnSceneChanged;
    }

    private void Start()
    {
        activityStruct = new Activity
        {
            Assets =
            {
                LargeImage = "aottg_2_title",
                LargeText = "AoTTG2"
            }
        };
        InMenu();
    }

    private void JoinViaDiscordInvite(string roomID)
    {
        PhotonNetwork.JoinRoom(roomID);
    }

    private void InMenu()
    {
        activityStruct.State = "In Menu";
        DiscordSocket.GetActivityManager().UpdateActivity(activityStruct, (result) =>
        {
            Debug.Log(result == Result.Ok
                ? $"Updated to Main Menu"
                : $"Failed to Update to Main Menu");
        });
    }

    public void SinglePlayerActivity(Room room)
    {
        activityStruct.State = "SinglePlayer!";
        activityStruct.Details = room.GetLevel() + " - " + room.GetGamemode();
        activityStruct.Secrets = new ActivitySecrets(); //Reset Secrets and Party structs.
        activityStruct.Party = new ActivityParty();

        DiscordSocket.GetActivityManager().UpdateActivity(activityStruct,
            (result) =>
            {
                Debug.Log(result == Result.Ok
                    ? $"Updated Single player presence."
                    : $"Failed to Update Single player presence.");
            });
    }

    public void UpdateMultiplayerActivity(Room room)
    {
        activityStruct.State = room.GetName() + " [" + PhotonNetwork.CloudRegion + "]";
        activityStruct.Details = room.GetLevel() + " - " + room.GetGamemode();
        activityStruct.Party = new ActivityParty
        {
            Size = new PartySize
            {
                CurrentSize = room.PlayerCount,
                MaxSize = room.MaxPlayers >= room.PlayerCount ? room.MaxPlayers : 10
            },
            Id = room.GetHashCode().ToString(),
        };
        activityStruct.Secrets = new ActivitySecrets
        {
            Join = room.Name,
        };

        DiscordSocket.GetActivityManager().UpdateActivity(activityStruct,
            (result) =>
            {
                Debug.Log(result == Result.Ok
                    ? $"Updated Multi Player Party."
                    : $"Failed to Update Multiplayer Party stats.");
            });
    }

    private void Update()
    {
        DiscordSocket.GetSocket().RunCallbacks();
    }

    private void OnApplicationQuit()
    {
        DiscordSocket.CloseSocket();
    }

    private void OnSceneChanged(Scene oldScene, Scene newScene)
    {
        if (newScene.buildIndex == 0)
        {
            InMenu();
        }
    }

    private static string GetApplicationPath()
    {
        string path = System.Environment.CurrentDirectory;
        path += "\\";
        if (Application.platform == RuntimePlatform.WindowsPlayer)
            path += "AoTTG 2.exe";
        else if (Application.platform == RuntimePlatform.OSXPlayer)
            path += "AoTTG 2.app";
        else
            throw new Exception("Unrecognized platform.");

        return path;
    }
}