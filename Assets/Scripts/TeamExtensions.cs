using ExitGames.Client.Photon;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

internal static class TeamExtensions
{
    public static PunTeams.Team GetTeam(this PhotonPlayer player)
    {
        object obj2;
        if (player.customProperties.TryGetValue("team", out obj2))
        {
            return (PunTeams.Team) ((byte) obj2);
        }
        return PunTeams.Team.none;
    }

    public static void SetTeam(this PhotonPlayer player, PunTeams.Team team)
    {
        if (!PhotonNetwork.connectedAndReady)
        {
            Debug.LogWarning("JoinTeam was called in state: " + PhotonNetwork.connectionStatesDetailed + ". Not connectedAndReady.");
        }
        if (PhotonNetwork.player.GetTeam() != team)
        {
            Hashtable propertiesToSet = new Hashtable();
            propertiesToSet.Add("team", (byte) team);
            PhotonNetwork.player.SetCustomProperties(propertiesToSet);
        }
    }
}

