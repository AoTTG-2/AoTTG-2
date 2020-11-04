using Assets.Scripts.Room;
using Discord;
using UnityEngine;

public class DiscordRichPresence : MonoBehaviour
{
    private Activity activityStruct;

    void Start()
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
        Debug.Log($"ROOM playre count = {room.PlayerCount} ROOM max Count = {room.MaxPlayers}");
        activityStruct.State = room.GetName() + " [" + PhotonNetwork.CloudRegion + "]";
        activityStruct.Details = room.GetLevel() + " - " + room.GetGamemode();
        activityStruct.Party = new ActivityParty
        {
            Size = new PartySize
            {
                CurrentSize = room.PlayerCount,
                MaxSize = 100
            }
        };
        
        DiscordSocket.GetActivityManager().UpdateActivity(activityStruct, (result) =>
        {
            if (result == Result.Ok)
            {
                Debug.Log($"Updated Party.");
            }
            else
            {
                Debug.Log($"Failed to Update Party stats.");
            }
        });

       // SetupJoinButton();
    }

    private void SetupJoinButton()
    {
        activityStruct.Secrets = new ActivitySecrets
        {
            Spectate = "",
            Join = "",
            Match = ""
        };
        DiscordSocket.GetActivityManager().UpdateActivity(activityStruct, (result) =>
        {
            if (result == Result.Ok)
            {
                Debug.Log($"Updated Join Button");
            }
            else
            {
                Debug.Log($"Failed to Update Join Button.");
            }
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

    void Update()
    {
        DiscordSocket.GetSocket().RunCallbacks();
    }

    void OnApplicationQuit()
    {
        DiscordSocket.CloseSocket();
    }
}