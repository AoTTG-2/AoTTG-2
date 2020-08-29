using System;
using Assets.Scripts.Gamemode.Settings;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Assets.Scripts.Settings.Titans;
using Newtonsoft.Json;

namespace Assets.Scripts.Settings
{
    public class GameSettings
    {
        public static PvPSettings PvP { get; private set; }
        public static GamemodeSettings Gamemode { get; private set; }
        public static SettingsTitan Titan { get; private set; }

        [JsonProperty]
        private List<GamemodeSettings> ConfigGamemodes { get; set; }
        [JsonProperty]
        private PvPSettings ConfigPvP { get; set; }
        [JsonProperty]
        private SettingsTitan ConfigTitan { get; set; }

        public void Initialize(List<GamemodeSettings> gamemodes, PvPSettings pvp, SettingsTitan titan)
        {
            PvP = ConfigPvP = pvp;
            Titan = ConfigTitan = titan;
            ConfigGamemodes = gamemodes;
        }

        public void ChangeSettings(GamemodeSettings levelGamemode)
        {
            Gamemode = levelGamemode;
            var playerGamemodeSettings = ConfigGamemodes.Single(x => x.GamemodeType == levelGamemode.GamemodeType);
            PvP = CreateFromObjects(ConfigPvP, playerGamemodeSettings.Pvp, levelGamemode.Pvp);
            Titan = CreateFromObjects(ConfigTitan, playerGamemodeSettings.Titan, levelGamemode.Titan);
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
                    if (p.GetValue(s).Equals(GetDefault(p.PropertyType)))
                    {
                        return false;
                    }

                    return true;
                }
                catch (NullReferenceException e)
                {
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
                    if (predicate(propertyInfo, source))
                    {
                        propertyInfo.SetValue(target, propertyInfo.GetValue(source));
                    }
                }
            }
        }

        private static object GetDefault(Type type)
        {
            if (type.IsValueType)
            {
                return Activator.CreateInstance(type);
            }
            return null;
        }
    }
}
