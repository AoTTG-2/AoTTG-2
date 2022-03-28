using Assets.Scripts.Settings.Game.Titans;
using Assets.Scripts.Settings.Types;
using Assets.Scripts.Settings.Validation;
using UnityEngine;

namespace Assets.Scripts.Settings.Game
{
    [CreateAssetMenu(fileName = "Custom", menuName = "Settings/Titan", order = 2)]
    public class TitanSettings : BaseSettings
    {
        [IntValidation(0, 100, 10)]
        public IntSetting Start;

        [IntValidation(0, 100, 50)] 
        public IntSetting Limit;

        public IntSetting MinimumDamage;
        public IntSetting MaximumDamage;

        public MindlessTitanSettings MindlessTitan;
        public FemaleTitanSettings FemaleTitan;
        public BaseTitanSettings ErenTitan;
        public BaseTitanSettings ColossalTitan;

        public override void Override(BaseSettings settings)
        {
            if (!(settings is TitanSettings titan)) return;
            if (titan.Start.HasValue) Start.Value = titan.Start.Value;
            if (titan.Limit.HasValue) Limit.Value = titan.Limit.Value;
            if (titan.MinimumDamage.HasValue) MinimumDamage.Value = titan.MinimumDamage.Value;
            if (titan.MaximumDamage.HasValue) MaximumDamage.Value = titan.MaximumDamage.Value;
            if (titan.MindlessTitan != null) MindlessTitan.Override(titan.MindlessTitan);
            if (titan.FemaleTitan != null) FemaleTitan.Override(titan.FemaleTitan);
            if (titan.ErenTitan != null) ErenTitan.Override(titan.ErenTitan);
            if (titan.ColossalTitan != null) ColossalTitan.Override(titan.ColossalTitan);
        }

        public override BaseSettings Copy()
        {
            var setting = Instantiate(this);
            if (MindlessTitan != null) setting.MindlessTitan = MindlessTitan.Copy() as MindlessTitanSettings;
            if (FemaleTitan != null) setting.FemaleTitan = FemaleTitan.Copy() as FemaleTitanSettings;
            if (ErenTitan != null) setting.ErenTitan = ErenTitan.Copy() as BaseTitanSettings;
            if (ColossalTitan != null) setting.ColossalTitan = ColossalTitan.Copy() as BaseTitanSettings;
            return setting;
        }
    }
}
