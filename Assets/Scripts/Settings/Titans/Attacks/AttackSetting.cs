using Assets.Scripts.Characters.Titan;
using Assets.Scripts.Gamemode;
using System.Collections.Generic;

namespace Assets.Scripts.Settings.Titans.Attacks
{
    public class AttackSetting
    {
        public static List<AttackSetting> GetAll<T>(Difficulty difficulty) where T : TitanBase
        {
            if (typeof(T) == typeof(MindlessTitan))
            {
                return new List<AttackSetting>
                {
                    new RockThrowSetting(difficulty)
                };
            }

            return null;
        }
    }
}
