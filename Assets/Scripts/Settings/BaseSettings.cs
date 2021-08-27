using Assets.Scripts.Settings.Game;
using Assets.Scripts.Settings.Types;
using Assets.Scripts.Settings.Validation;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Settings
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

        /// <summary>
        /// Initializes properties with optional validation attributes
        /// </summary>
        public void Initialize()
        {
            var properties = GetType().GetFields()
                .Where(prop => Attribute.IsDefined(prop, typeof(ValidationAttribute)));

            foreach (var field in properties)
            {
                var fieldType = field.FieldType;
                if (fieldType == typeof(IntSetting))
                {
                    var attribute = (IntValidationAttribute) Attribute.GetCustomAttribute(field, typeof(IntValidationAttribute));
                    if (attribute == null) continue;
                    var method = fieldType.GetMethod(nameof(IntSetting.Setup));
                    if (method == null)
                    {
                        Debug.LogError($"Settings: Could not find method {nameof(IntSetting.Setup)}");
                        continue;
                    }

                    var value = field.GetValue(this);
                    if (value == null) continue;
                    method.Invoke(value, new object[] { attribute.MinValue, attribute.MaxValue, attribute.Default });
                }
                else if (fieldType == typeof(StringSetting))
                {
                    var attribute = (StringValidationAttribute) Attribute.GetCustomAttribute(field, typeof(StringValidationAttribute));
                    if (attribute == null) continue;
                    var method = fieldType.GetMethod(nameof(StringSetting.Setup));
                    if (method == null)
                    {
                        Debug.LogError($"Settings: Could not find method {nameof(StringSetting.Setup)}");
                        continue;
                    }

                    var value = field.GetValue(this);
                    if (value == null) continue;
                    method.Invoke(value, new object[] { attribute.MaxLength });
                }
            }
        }

        /// <summary>
        /// Returns a copy by using Unity's <see cref="UnityEngine.Object.Instantiate"/>
        /// </summary>
        /// <returns></returns>
        public virtual BaseSettings Copy()
        {
            return Instantiate(this);
        }

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

            if (this is TimeSettings)
            {
                Setting.Gamemode.Time.Override(this);
            }
        }
#endif
    }
}
