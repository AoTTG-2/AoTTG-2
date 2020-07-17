using Assets.Scripts.Room;
using Discord;
using UnityEngine;

public class DiscordRichPresence : MonoBehaviour {

    void Start()
    {
        InMenu();
    }

	public void InMenu()
    {
        var activity = new Activity
        {
            State = "In Menu",
            Assets =
            {
                LargeImage = "aottg_2_title",
                LargeText = "AoTTG2",
            },
        };
        DiscordSocket.GetActivityManager().UpdateActivity(activity, (result) =>
        {
            if(result == Discord.Result.Ok)
            {
                Debug.Log("Working!");
            }
            else
            {
                Debug.LogError("Not Working!");
            }
        });
    }

    public void InGame(RoomInfo room)
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
	
	void Update () {
        DiscordSocket.GetSocket().RunCallbacks();
	}

    void OnApplicationQuit()
    {
        DiscordSocket.CloseSocket();
    }
}
