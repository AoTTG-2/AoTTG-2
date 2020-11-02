using Assets.Scripts.Settings.Gamemodes;
using System.Linq;

namespace Assets.Scripts.Room
{
    public static class RoomExtensions
    {
        public static string GetName(this RoomInfo room)
        {
            return room.CustomProperties["name"].ToString();
        }

        public static string GetLevel(this RoomInfo room)
        {
            return room.CustomProperties["level"].ToString();
        }

        public static string GetGamemode(this RoomInfo room)
        {
            return room.CustomProperties["gamemode"].ToString();
        }

        public static Level GetLevel(this global::Room room)
        {
            var level = room.CustomProperties["level"].ToString();
            return LevelBuilder.GetAllLevels().Single(x => x.Name == level);
        }

        /// <summary>
        /// Check if a password is required to join the room. Note: This property is only used for UX purposes. Photon Server contains the knowledge
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        public static bool IsPasswordRequired(this RoomInfo room)
        {
            if (!room.CustomProperties.ContainsKey("passworded")) return false;
            return (bool)room.CustomProperties["passworded"];
        }

        /// <summary>
        /// Check if an account is required to join the room. Note: This property is only used for UX purposes. Photon Server contains the knowledge
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        public static bool IsAccountRequired(this RoomInfo room)
        {
            if (!room.CustomProperties.ContainsKey("account")) return false;
            return (bool) room.CustomProperties["account"];
        }


        public static GamemodeSettings GetGamemodeSetting(this global::Room room, Level level)
        {
            var gamemode = room.CustomProperties["gamemode"].ToString();
            return level.Gamemodes.Single(x => x.Name == gamemode);
        }
    }
}
