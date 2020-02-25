using ExitGames.Client.Photon;
using System;

public class RoomOptions
{
    public bool cleanupCacheOnLeave = PhotonNetwork.autoCleanUpPlayerObjects;
    public Hashtable customRoomProperties;
    public string[] customRoomPropertiesForLobby = new string[0];
    public bool isOpen = true;
    public bool isVisible = true;
    public int maxPlayers;
}

