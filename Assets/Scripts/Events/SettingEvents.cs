using Assets.Scripts.Settings;
using Assets.Scripts.Settings.Gamemodes;
using Assets.Scripts.Settings.Titans;

namespace Assets.Scripts.Events
{
    public delegate void OnPvpSettingsChanged(PvPSettings settings);

    public delegate void OnGamemodeSettingsChanged(GamemodeSettings settings);

    public delegate void OnTitanSettingsChanged(SettingsTitan settings);

    public delegate void OnTimeSettingsChanged(TimeSettings settings);

    public delegate void OnHorseSettingsChanged(HorseSettings settings);

    public delegate void OnRespawnSettingsChanged(RespawnSettings settings);

    public delegate void OnGlobalSettingsChanged(GlobalSettings settings);
}
