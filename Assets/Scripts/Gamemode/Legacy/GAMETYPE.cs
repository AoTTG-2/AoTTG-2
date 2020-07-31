using System;
using UnityEngine;

public enum GAMETYPE
{
    [Obsolete("Use PhotonNetwork.OfflineMode instead as per #184 Singleplayer", false)]
    SINGLE,
    Stop,
    Playing,
    [Obsolete("This is almost always true", false)]
    MULTIPLAYER
}

