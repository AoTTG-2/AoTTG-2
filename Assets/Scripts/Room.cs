using ExitGames.Client.Photon;
using UnityEngine;

public class Room : RoomInfo
{
    internal Room(string roomName, RoomOptions options) : base(roomName, null)
    {
        if (options == null)
        {
            options = new RoomOptions();
        }
        visibleField = options.isVisible;
        openField = options.isOpen;
        maxPlayersField = (byte) options.maxPlayers;
        autoCleanUpField = false;
        CacheProperties(options.customRoomProperties);
        propertiesListedInLobby = options.customRoomPropertiesForLobby;
    }

    public void SetCustomProperties(Hashtable propertiesToSet)
    {
        if (propertiesToSet != null)
        {
            customProperties.MergeStringKeys(propertiesToSet);
            customProperties.StripKeysWithNullValues();
            Hashtable gameProperties = propertiesToSet.StripToStringKeys();
            if (!PhotonNetwork.offlineMode)
            {
                PhotonNetwork.networkingPeer.OpSetCustomPropertiesOfRoom(gameProperties, true, 0);
            }
            object[] parameters = { propertiesToSet };
            NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnPhotonCustomRoomPropertiesChanged, parameters);
        }
    }

    public void SetPropertiesListedInLobby(string[] propsListedInLobby)
    {
        Hashtable gameProperties = new Hashtable();
        gameProperties[(byte) 250] = propsListedInLobby;
        PhotonNetwork.networkingPeer.OpSetPropertiesOfRoom(gameProperties, false, 0);
        propertiesListedInLobby = propsListedInLobby;
    }

    public override string ToString()
    {
        object[] args = { nameField, !visibleField ? "hidden" : "visible", !openField ? "closed" : "open", maxPlayersField, playerCount };
        return string.Format("Room: '{0}' {1},{2} {4}/{3} players.", args);
    }

    public new string ToStringFull()
    {
        object[] args = { nameField, !visibleField ? "hidden" : "visible", !openField ? "closed" : "open", maxPlayersField, playerCount, customProperties.ToStringFull() };
        return string.Format("Room: '{0}' {1},{2} {4}/{3} players.\ncustomProps: {5}", args);
    }

    public bool autoCleanUp
    {
        get
        {
            return autoCleanUpField;
        }
    }

    public new int maxPlayers
    {
        get
        {
            return maxPlayersField;
        }
        set
        {
            if (!Equals(PhotonNetwork.room))
            {
                Debug.LogWarning("Can't set maxPlayers when not in that room.");
            }
            if (value > 0xff)
            {
                Debug.LogWarning("Can't set Room.maxPlayers to: " + value + ". Using max value: 255.");
                value = 0xff;
            }
            if ((value != maxPlayersField) && !PhotonNetwork.offlineMode)
            {
                Hashtable gameProperties = new Hashtable();
                gameProperties.Add((byte) 0xff, (byte) value);
                PhotonNetwork.networkingPeer.OpSetPropertiesOfRoom(gameProperties, true, 0);
            }
            maxPlayersField = (byte) value;
        }
    }

    public new string name
    {
        get
        {
            return nameField;
        }
        internal set
        {
            nameField = value;
        }
    }

    public new bool open
    {
        get
        {
            return openField;
        }
        set
        {
            if (!Equals(PhotonNetwork.room))
            {
                Debug.LogWarning("Can't set open when not in that room.");
            }
            if ((value != openField) && !PhotonNetwork.offlineMode)
            {
                Hashtable gameProperties = new Hashtable();
                gameProperties.Add((byte) 0xfd, value);
                PhotonNetwork.networkingPeer.OpSetPropertiesOfRoom(gameProperties, true, 0);
            }
            openField = value;
        }
    }

    public new int playerCount
    {
        get
        {
            if (PhotonNetwork.playerList != null)
            {
                return PhotonNetwork.playerList.Length;
            }
            return 0;
        }
    }

    public string[] propertiesListedInLobby { get; private set; }

    public new bool visible
    {
        get
        {
            return visibleField;
        }
        set
        {
            if (!Equals(PhotonNetwork.room))
            {
                Debug.LogWarning("Can't set visible when not in that room.");
            }
            if ((value != visibleField) && !PhotonNetwork.offlineMode)
            {
                Hashtable gameProperties = new Hashtable();
                gameProperties.Add((byte) 0xfe, value);
                PhotonNetwork.networkingPeer.OpSetPropertiesOfRoom(gameProperties, true, 0);
            }
            visibleField = value;
        }
    }
}