using Assets.Scripts.Room;
using Discord;
using UnityEngine;

public class DiscordRichPresence : MonoBehaviour
{
    private Activity activityStruct;

    private void Start()
    {
        activityStruct = new Activity
        {
            Assets =
            {
                LargeImage = "aottg_2_title",
                LargeText = "AoTTG2",
            },
        };
        InMenu();
    }

    public void InMenu()
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

    public void InMultiplayerGame(Room room)
    {
        Debug.Log($"Current room player count = {room.PlayerCount} and max players = {room.MaxPlayers}");
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


    public void InGame(Room room)
    {
        var activity = new Activity
        {
            State = room.GetName() + " [" + PhotonNetwork.CloudRegion + "]",
            Details = room.GetLevel() + " - " + room.GetGamemode(),
            Party =
            {
                Size =
                {
                    CurrentSize = room.PlayerCount,
                    MaxSize = room.MaxPlayers,
                },
            },
            Assets =
            {
                LargeImage = "aottg_2_title",
                LargeText = "AoTTG2",
            },
        };
        DiscordSocket.GetActivityManager().UpdateActivity(activity, (result) =>
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

    private void Update()
    {
        DiscordSocket.GetSocket().RunCallbacks();
    }

    private void OnApplicationQuit()
    {
        DiscordSocket.CloseSocket();
    }
}