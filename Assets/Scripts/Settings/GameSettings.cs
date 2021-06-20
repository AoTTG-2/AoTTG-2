using Assets.Scripts.Services;
using Assets.Scripts.Services.Interface;
using Assets.Scripts.Settings.Gamemodes;
using Assets.Scripts.Settings.Titans;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Debug = UnityEngine.Debug;

namespace Assets.Scripts.Settings
{
    public class GameSettings
    {
        protected ISettingsService SettingsService => Service.Settings;

        public static PvPSettings PvP { get; private set; }
        public static GamemodeSettings Gamemode { get; private set; }
        public static GlobalSettings Global { get; private set; }

        public static T DerivedGamemode<T>() where T : GamemodeSettings
        {
            return Gamemode as T;
        }
        public static SettingsTitan Titan { get; private set; }
        public static HorseSettings Horse { get; private set; }
        public static RespawnSettings Respawn { get; private set; }
        public static TimeSettings Time { get; private set; }

        [JsonProperty("Gamemodes")]
        private List<GamemodeSettings> ConfigGamemodes { get; set; }

        [JsonProperty("PvP")]
        private PvPSettings ConfigPvP { get; set; }

        [JsonProperty("Titan")]
        private SettingsTitan ConfigTitan { get; set; }

        [JsonProperty("Horse")]
        private HorseSettings ConfigHorse { get; set; }

        [JsonProperty("Respawn")]
        private RespawnSettings ConfigRespawn { get; set; }

        [JsonProperty("Time")]
        private TimeSettings ConfigTime { get; set; }

        [JsonProperty("Global")]
        private GlobalSettings ConfigGlobal { get; set; }

        /// <summary>
        /// Update the GameSettings based on the static definitions
        /// </summary>
        public void Update()
        {
            var gamemodes = ConfigGamemodes.ToList();
            if (Gamemode != null)
            {
                
                var gamemode = gamemodes.Single(x => x.GamemodeType == Gamemode.GamemodeType);
                gamemodes.Remove(gamemode);
                gamemodes.Add(Gamemode);
            }
            
            ConfigGamemodes = gamemodes;
            ConfigPvP = PvP;
            ConfigTitan = Titan;
            ConfigHorse = Horse;
            ConfigRespawn = Respawn;
            Time.LastModified = DateTime.UtcNow;
            ConfigTime = Time;
            ConfigGlobal = Global;
        }

        /// <summary>
        /// Update the GamemodeSettings and synchronize to all players
        /// </summary>
        /// <param name="settings"></param>
        public void Update(GamemodeSettings settings)
        {
            var gamemodes = ConfigGamemodes.ToList();
            var gamemode = gamemodes.Single(x => x.GamemodeType == settings.GamemodeType);
            gamemodes.Remove(gamemode);
            gamemodes.Add(settings);
            ConfigGamemodes = gamemodes;

            if (Gamemode.GamemodeType == settings.GamemodeType)
            {
                Gamemode = settings;
            }
            SettingsService.SyncSettings();
        }

        /// <summary>
        /// Update the PvPSettings and synchronize to all players
        /// </summary>
        /// <param name="settings"></param>
        public void Update(PvPSettings settings)
        {
            PvP = ConfigPvP = settings;
            SettingsService.SyncSettings();
        }

        /// <summary>
        /// Update the Titan Settings and synchronize to all players
        /// </summary>
        /// <param name="settings"></param>
        public void Update(SettingsTitan settings)
        {
            Titan = ConfigTitan = settings;
            SettingsService.SyncSettings();
        }

        /// <summary>
        /// Update the Horse Settings and synchronize to all players
        /// </summary>
        /// <param name="settings"></param>
        public void Update(HorseSettings settings)
        {
            Horse = ConfigHorse = settings;
            SettingsService.SyncSettings();
        }

        /// <summary>
        /// Update the Respawn Settings and synchronize to all players
        /// </summary>
        /// <param name="settings"></param>
        public void Update(RespawnSettings settings)
        {
            Respawn = ConfigRespawn = settings;
            SettingsService.SyncSettings();
        }

        /// <summary>
        /// Update the Time Settings and synchronize to all players
        /// </summary>
        /// <param name="settings"></param>
        public void Update(TimeSettings settings)
        {
            Time = ConfigTime = settings;
            SettingsService.SyncSettings();
        }

        /// <summary>
        /// Update the Global Settings and synchronize to all players
        /// </summary>
        /// <param name="settings"></param>
        public void Update(GlobalSettings settings)
        {
            Global = ConfigGlobal = settings;
            SettingsService.SyncSettings();
        }

        public void Initialize(GamemodeType type)
        {
            Gamemode = ConfigGamemodes.Single(x => x.GamemodeType == type);
        }

        public void Initialize(string json)
        {
            var gameSettings = JsonConvert.DeserializeObject<GameSettings>(json);
            Initialize(gameSettings.ConfigGamemodes, gameSettings.ConfigPvP, gameSettings.ConfigTitan, gameSettings.ConfigHorse, gameSettings.ConfigRespawn, gameSettings.ConfigTime, gameSettings.ConfigGlobal);
        }

        public void Initialize(List<GamemodeSettings> gamemodes, PvPSettings pvp, SettingsTitan titan, HorseSettings horse, RespawnSettings respawn, TimeSettings time, GlobalSettings global)
        {
            PvP = ConfigPvP = pvp;
            Titan = ConfigTitan = titan;
            ConfigGamemodes = gamemodes;
            if (FengGameManagerMKII.Gamemode != null)
                Gamemode = ConfigGamemodes.Single(x => x.GamemodeType == FengGameManagerMKII.Gamemode.GamemodeType);
            Horse = ConfigHorse = horse;
            Respawn = ConfigRespawn = respawn;
            Time = ConfigTime = time;
            Global = ConfigGlobal = global;
        }
        
        public void ChangeSettings(GamemodeSettings levelGamemode)
        {
            var playerGamemodeSettings = ConfigGamemodes.Single(x => x.GamemodeType == levelGamemode.GamemodeType);
            switch (levelGamemode.GamemodeType)
            {
                case GamemodeType.Titans:
                    Gamemode = CreateFromObjects(playerGamemodeSettings as KillTitansSettings, levelGamemode as KillTitansSettings);
                    break;
                case GamemodeType.Endless:
                    Gamemode = CreateFromObjects(playerGamemodeSettings as EndlessSettings, levelGamemode as EndlessSettings);
                    break;
                case GamemodeType.Catch:
                    Gamemode = CreateFromObjects(playerGamemodeSettings as CatchGamemodeSettings, levelGamemode as CatchGamemodeSettings);
                    break;
                case GamemodeType.Capture:
                    Gamemode = CreateFromObjects(playerGamemodeSettings as CaptureGamemodeSettings, levelGamemode as CaptureGamemodeSettings);
                    break;
                case GamemodeType.Wave:
                    Gamemode = CreateFromObjects(playerGamemodeSettings as WaveGamemodeSettings, levelGamemode as WaveGamemodeSettings);
                    break;
                case GamemodeType.Racing:
                    Gamemode = CreateFromObjects(playerGamemodeSettings as RacingSettings, levelGamemode as RacingSettings);
                    break;
                case GamemodeType.Trost:
                    Gamemode = CreateFromObjects(playerGamemodeSettings as TrostSettings, levelGamemode as TrostSettings);
                    break;
                case GamemodeType.TitanRush:
                    Gamemode = CreateFromObjects(playerGamemodeSettings as RushSettings, levelGamemode as RushSettings);
                    break;
                case GamemodeType.PvpAhss:
                    Gamemode = CreateFromObjects(playerGamemodeSettings as PvPAhssSettings, levelGamemode as PvPAhssSettings);
                    break;
                case GamemodeType.Infection:
                    Gamemode = CreateFromObjects(playerGamemodeSettings as InfectionGamemodeSettings, levelGamemode as InfectionGamemodeSettings);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(levelGamemode.GetType().ToString());
            }
            PvP = CreateFromObjects(ConfigPvP, playerGamemodeSettings.Pvp, levelGamemode.Pvp);
            Titan = CreateFromObjects(ConfigTitan, playerGamemodeSettings.Titan, levelGamemode.Titan);

            Titan.Mindless = CreateFromObjects(ConfigTitan.Mindless, playerGamemodeSettings.Titan?.Mindless, levelGamemode.Titan?.Mindless);
            Titan.Colossal = CreateFromObjects(ConfigTitan.Colossal, playerGamemodeSettings.Titan?.Colossal, levelGamemode.Titan?.Colossal);
            Titan.Female = CreateFromObjects(ConfigTitan.Female, playerGamemodeSettings.Titan?.Female, levelGamemode.Titan?.Female);
            Titan.Eren = CreateFromObjects(ConfigTitan.Eren, playerGamemodeSettings.Titan?.Eren, levelGamemode.Titan?.Eren);

            Horse = CreateFromObjects(ConfigHorse, playerGamemodeSettings.Horse, levelGamemode.Horse);
            Respawn = CreateFromObjects(ConfigRespawn, playerGamemodeSettings.Respawn, levelGamemode.Respawn);
            Time = CreateFromObjects(ConfigTime, playerGamemodeSettings.Time, levelGamemode.Time);
            FengGameManagerMKII.instance.OnRoomSettingsInitialized();
        }

        public T CreateFromObjects<T>(params T[] sources)
            where T : new()
        {
            var ret = new T();
            MergeObjects(ret, sources);

            return ret;
        }

        public void MergeObjects<T>(T target, params T[] sources)
        {
            Func<PropertyInfo, T, bool> predicate = (p, s) =>
            {
                try
                {
                    return !p.GetValue(s).Equals(GetDefault(p.PropertyType));
                }
                catch (NullReferenceException)
                {
                    Debug.LogWarning($"{p.Name} of {p.PropertyType} is unassigned in {s}");
                    return false;
                }
            };
            MergeObjects(target, predicate, sources);
        }

        public void MergeObjects<T>(T target, Func<PropertyInfo, T, bool> predicate, params T[] sources)
        {
            foreach (var propertyInfo in typeof(T).GetProperties().Where(prop => prop.CanRead && prop.CanWrite))
            {
                foreach (var source in sources)
                {
                    if (source == null)
                        continue;
                    if (predicate(propertyInfo, source))
                    {
                        propertyInfo.SetValue(target, propertyInfo.GetValue(source));
                    }
                }
            }
        }

        private static object GetDefault(Type type)
        {
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }
    }
}
