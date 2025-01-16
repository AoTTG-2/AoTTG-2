using Assets.Scripts.Settings.Game.Gamemodes;

namespace Assets.Scripts.Services.Interface
{
    public interface ISettingsService
    {
        /// <summary>
        /// Returns the current Setting.Gamemode as an object
        /// </summary>
        /// <returns></returns>
        GamemodeSetting Get();

        /// <summary>
        /// Sets the "Settings" room property when Master Client, or retrieves settings from this room property when not a Master Client
        /// </summary>
        void SetRoomPropertySettings();

        /// <summary>
        /// Sets the Setting.Gamemode GamemodeSettings based on the GamemodeType
        /// </summary>
        /// <param name="type"></param>
        void SetGamemodeType(GamemodeType type);

        /// <summary>
        /// Synchronize the existing Setting.Gamemode to everyone
        /// </summary>
        void SyncSettings();

        /// <summary>
        /// Synchronize the Setting.Gamemode to everyone
        /// </summary>
        void SyncSettings(GamemodeSetting settings);
        
        /// <summary>
        /// Synchronize the raw Setting.Gamemode json to everyone
        /// </summary>
        void SyncSettings(string json);

        void SyncSettingsRpc(string settings, PhotonMessageInfo info);
    }
}
