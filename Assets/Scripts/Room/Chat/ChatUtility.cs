namespace Assets.Scripts.Room.Chat
{
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
        public static string FormatTextColorCyan(string input)
        {
            return $"<color=#00FFFF>{input}</color>";
        }

        /// <summary>
        /// Formats text as <color=#FF00FF>{input}</color>
        /// </summary>
        /// <param name="input"></param>
        /// <returns><color=#FF00FF>{input}</color></returns>
        public static string FormatTextColorMagenta(string input)
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
    }
}

