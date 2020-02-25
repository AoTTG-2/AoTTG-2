using ExitGames.Client.Photon;
using System;
using System.Runtime.CompilerServices;

internal static class ScoreExtensions
{
    public static void AddScore(this PhotonPlayer player, int scoreToAddToCurrent)
    {
        int num = player.GetScore() + scoreToAddToCurrent;
        Hashtable propertiesToSet = new Hashtable();
        propertiesToSet["score"] = num;
        player.SetCustomProperties(propertiesToSet);
    }

    public static int GetScore(this PhotonPlayer player)
    {
        object obj2;
        if (player.customProperties.TryGetValue("score", out obj2))
        {
            return (int) obj2;
        }
        return 0;
    }

    public static void SetScore(this PhotonPlayer player, int newScore)
    {
        Hashtable propertiesToSet = new Hashtable();
        propertiesToSet["score"] = newScore;
        player.SetCustomProperties(propertiesToSet);
    }
}

