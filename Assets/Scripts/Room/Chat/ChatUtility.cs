using static FengGameManagerMKII;

/// <summary>
/// String
/// </summary>
public static class ChatUtility
{
    /// <summary>
    /// Formats text as <color=#00FFFF>{input}</color>
    /// </summary>
    /// <param name="input"></param>
    /// <returns><color=#00FFFF>{input}</color></returns>
    public static string FormatTextColor00FFFF(string input)
    {
        return $"<color=#00FFFF>{input}</color>";
    }

    /// <summary>
    /// Formats text as <color=#FF00FF>{input}</color>
    /// </summary>
    /// <param name="input"></param>
    /// <returns><color=#FF00FF>{input}</color></returns>
    public static string FormatTextColorFF00FF(string input)
    {
        return $"<color=#FF00FF>{input}</color>";
    }

    /// <summary>
    /// Formats text as <color=#FFCC00>{input}</color>
    /// </summary>
    /// <param name="input"></param>
    /// <returns><color=#FFCC00>{input}</color></returns>
    public static string FormatSystemMessage(string input)
    {
        return $"<color=#FFCC00>{input}</color>";
    }

    /// <summary>
    /// Gets the name of PhotonPlayer.player
    /// </summary>
    /// <param name="player"></param>
    /// <returns>PhotonPlayerProperty.name</returns>
    public static string GetPlayerName(PhotonPlayer player)
    {
        return RCextensions.returnStringFromObject(player.CustomProperties[PhotonPlayerProperty.name]);
    }

    /// <summary>
    /// sets color of name in chat depending on what team PhotonPlayer.player is in
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public static string SetNameColorDependingOnteam(PhotonPlayer player)
    {
        var name = GetPlayerName(player);
        var playerTeam = player.GetTeam();
        switch (playerTeam)
        {
            case PunTeams.Team.red:
                name = FormatTextColor00FFFF(name);
                break;
            case PunTeams.Team.blue:
                name = FormatTextColorFF00FF(name);
                break;
            default:
                break;
        }

        return name;
    }
}

