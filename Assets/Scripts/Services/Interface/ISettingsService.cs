using Assets.Scripts.Events;
using Assets.Scripts.Gamemode;
using Assets.Scripts.Settings;

namespace Assets.Scripts.Services.Interface
{
    public interface ISettingsService
    {
        event OnGamemodeSettingsChanged OnGamemodeSettingsChanged;
        event OnPvpSettingsChanged OnPvpSettingsChanged;
        event OnHorseSettingsChanged OnHorseSettingsChanged;
        event OnRespawnSettingsChanged OnRespawnSettingsChanged;
        event OnTitanSettingsChanged OnTitanSettingsChanged;
        event OnGlobalSettingsChanged OnGlobalSettingsChanged;
        event OnTimeSettingsChanged OnTimeSettingsChanged;

        /// <summary>
        /// Returns the current GameSettings as an object
        /// </summary>
        /// <returns></returns>
        GameSettings Get();

        /// <summary>
        /// Sets the "Settings" room property when Master Client, or retrieves settings from this room property when not a Master Client
        /// </summary>
        void SetRoomPropertySettings();

        /// <summary>
        /// Sets the GameSettings GamemodeSettings based on the GamemodeType
        /// </summary>
        /// <param name="type"></param>
        void SetGamemodeType(GamemodeType type);

        /// <summary>
        /// Synchronize the existing GameSettings to everyone
        /// </summary>
        void SyncSettings();

        /// <summary>
        /// Synchronize the GameSettings to everyone
        /// </summary>
        void SyncSettings(GameSettings settings);

        /// <summary>
        /// Synchronize the GameSettings based on difficulty to everyone
        /// </summary>
        void SyncSettings(Difficulty difficulty);

        /// <summary>
        /// Synchronize the raw GameSettings json to everyone
        /// </summary>
        void SyncSettings(string json);

        void SyncSettingsRpc(string settings, PhotonMessageInfo info);
        void SyncSettingsRpc(Difficulty difficulty, PhotonMessageInfo info);
    }
}
