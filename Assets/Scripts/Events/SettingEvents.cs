using Assets.Scripts.Settings.Game;

namespace Assets.Scripts.Events
{
    public delegate void OnPvpSettingsChanged(PvPSettings settings);
    
    public delegate void OnTimeSettingsChanged(TimeSettings settings);

    public delegate void OnHorseSettingsChanged(HorseSettings settings);

    public delegate void OnRespawnSettingsChanged(RespawnSettings settings);

    public delegate void OnGlobalSettingsChanged(GlobalSettings settings);
}
