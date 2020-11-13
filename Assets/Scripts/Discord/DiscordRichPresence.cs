using System.Collections.Concurrent;
using Assets.Scripts.Room;
using Discord;
using UnityEngine;

public class DiscordRichPresence : MonoBehaviour
{
    private Activity activityStruct;

    private void Start()
    {
        DiscordSocket.GetActivityManager().RegisterCommand(GetEXEPath());
        activityStruct = new Activity
        {
            Assets =
            {
                LargeImage = "aottg_2_title",
                LargeText = "AoTTG2",
            },
        };
        InMenu();
        DiscordSocket.GetActivityManager().OnActivityJoin += JoinRoom;
    }

    private string GetEXEPath()
    {
        string path = System.Environment.CurrentDirectory;
        path+="\\";
        path += "AoTTG 2.exe";
        Debug.Log($"EXE PATH = {path}");

        return path;
    }

    private void JoinRoom(string temp)
    {
        PhotonNetwork.JoinRoom(temp);
    }

    private void InMenu()
    {
        activityStruct.State = "In Menu";
        DiscordSocket.GetActivityManager().UpdateActivity(activityStruct, (result) =>
        {
            if (result == Discord.Result.Ok)
            {
                Debug.Log("Working!");
            }
            else
            {
                Debug.LogError("Not Working!");
            }
        });
    }

    public void UpdateActivity(Room room)
    {
        activityStruct.State = room.GetName() + " [" + PhotonNetwork.CloudRegion + "]";
        activityStruct.Details = room.GetLevel() + " - " + room.GetGamemode();
        activityStruct.Party = new ActivityParty
        {
            Size = new PartySize {CurrentSize = room.PlayerCount, MaxSize = 100}, 
            Id = room.GetHashCode().ToString(),
        };
        activityStruct.Secrets = new ActivitySecrets
        {
            Join = room.Name,
        };

        DiscordSocket.GetActivityManager().UpdateActivity(activityStruct, (result) =>
        {
            Debug.Log(result == Result.Ok ? $"Updated Party." : $"Failed to Update Party stats.");
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
}