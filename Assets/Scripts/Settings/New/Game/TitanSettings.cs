using Assets.Scripts.Settings.New.Game.Titans;
using Assets.Scripts.Settings.New.Types;
using Assets.Scripts.Settings.New.Validation;
using System;
using UnityEngine;

namespace Assets.Scripts.Settings.New.Game
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
