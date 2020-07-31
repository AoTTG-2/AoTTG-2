using System;
using UnityEngine;

[AddComponentMenu("Camera-Control/Mouse Look")]
public enum GAMETYPE
{
    [Obsolete("Use PhotonNetwork.OfflineMode instead as per #184 Singleplayer", true)]
    SINGLE,
    SERVER,
    CLIENT,
    STOP,
    MULTIPLAYER
}

