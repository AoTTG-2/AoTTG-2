using Assets.Scripts.Gamemode.Options;
using Assets.Scripts.Settings.Types;
using Newtonsoft.Json;
using UnityEngine;

namespace Assets.Scripts.Settings.Game.Titans
{
    [CreateAssetMenu(fileName = "Custom", menuName = "Settings/Titan/Base Titan", order = 2)]
    public class BaseTitanSettings : BaseSettings
    {
        public BoolSetting Enabled;
        public FloatSetting SizeMinimum;
        public FloatSetting SizeMaximum;
        public FloatSetting ChaseDistance;
        public EnumSetting<TitanHealthMode> HealthMode;
        public IntSetting HealthMinimum;
        public IntSetting HealthMaximum;
        public IntSetting HealthRegeneration;
        public IntSetting ExplodeMode;
        public FloatSetting Idle;
        public FloatSetting Speed;
        public FloatSetting RunSpeed;

        [JsonIgnore]
        public float? Size => SizeMinimum.HasValue && SizeMaximum.HasValue
            ? Random.Range(SizeMinimum.Value, SizeMaximum.Value)
            : (float?) null;

        [JsonIgnore]
        public int Health => Random.Range(HealthMinimum.Value, HealthMaximum.Value);

        public override void Override(BaseSettings settings)
        {
            
        }
    }
}
