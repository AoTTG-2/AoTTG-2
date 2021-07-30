using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Settings.New
{
    public abstract class BaseSettings : ScriptableObject
    {
        public string Name;
        public DateTime LastModified;

        protected string SettingsFolder => 
            $"{Application.streamingAssetsPath}{Path.AltDirectorySeparatorChar}Settings{Path.AltDirectorySeparatorChar}{GetType().Name}";

        /// <summary>
        /// Saves the current <see cref="BaseSettings"/> to JSON and writes it to <see cref="SettingsFolder"/>
        /// </summary>
        public void Save()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                Name = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
            }

            LastModified = DateTime.UtcNow;

            var path = SettingsFolder;
            Directory.CreateDirectory(path);
            var file = $"{SettingsFolder}{Path.AltDirectorySeparatorChar}{Name}.json";
            var json = JsonConvert.SerializeObject(this);
            File.WriteAllText(file, json);
        }

        /// <summary>
        /// Will attempt to load a ScriptableObject by <paramref name="settingsName"/> and execute <see cref="Override"/>
        /// </summary>
        /// <param name="settingsName"></param>
        public void Load(string settingsName)
        {
            if (string.IsNullOrEmpty(settingsName)) throw new ArgumentNullException(settingsName);

            var file = $"{SettingsFolder}{Path.AltDirectorySeparatorChar}{settingsName}.json";
            var content = File.ReadAllText(file);
            if (string.IsNullOrEmpty(content))
                throw new FileNotFoundException($"{file} does not exist or does not have any content");

            var data = JsonConvert.DeserializeObject(content, GetType()) as BaseSettings;
            if (data == null)
                throw new Exception($"The loaded settings are not of type: {GetType().Name}");

            Override(data);
        }

        //TODO: Figure out some generic method, so non-null values will be overriden, whereas null values will be ignored.
        /// <summary>
        /// Overrides the Settings on one class with another
        /// </summary>
        /// <param name="settings"></param>
        public abstract void Override(BaseSettings settings);

#if UNITY_EDITOR
        /// <summary>
        /// Editor-only. Forces the existing Setting to be overridden by the current selected ScriptableObject
        /// </summary>
        public void ForceLocalOverride()
        {
            if (this is DebugSettings)
            {
                Setting.Debug.Override(this);
            }
        }
#endif
    }
}
