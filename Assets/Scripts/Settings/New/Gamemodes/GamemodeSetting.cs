using Assets.Scripts.Gamemode.Options;
using Assets.Scripts.Settings.New.Types;
using Assets.Scripts.Settings.New.Validation;
using System;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Settings.New.Gamemodes
{
    public abstract class GamemodeSetting : BaseSettings
    {
        public GamemodeType GamemodeType { get; protected set; }


        public StringSetting Description;
        public BoolSetting LavaMode;
        public EnumSetting<TeamMode> Single;
        public MultipleEnumSetting<TeamMode> TeamMode;
        [IntValidation(0, 100, 50)]
        public IntSetting Test;

        public override void Override(BaseSettings settings)
        {
            if (!(settings is GamemodeSetting gamemodeSetting)) return;

            if (gamemodeSetting.Test?.HasValue == true) Test = gamemodeSetting.Test;
        }

        public override void Initialize()
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
    }
}
