using Discord;
using Photon;
using System;
using UnityEditor;

public static class DiscordSocket {
    private static Discord.Discord discord;

    static DiscordSocket()
    {
        discord = new Discord.Discord(730150236185690172, (UInt64) Discord.CreateFlags.Default);
    }
	
    public static Discord.Discord GetSocket()
    {
        return discord;
    }

    public static void CloseSocket()
    {
        discord.Dispose();
    }

    public static ActivityManager GetActivityManager()
    {
        return discord.GetActivityManager();
    }
}
