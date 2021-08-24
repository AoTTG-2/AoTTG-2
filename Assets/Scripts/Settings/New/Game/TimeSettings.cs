using Assets.Scripts.Settings.New.Types;
using UnityEngine;

namespace Assets.Scripts.Settings.New.Game
{
    [CreateAssetMenu(fileName = "Custom", menuName = "Settings/Time", order = 2)]
    public class TimeSettings : BaseSettings
    {
        public FloatSetting CurrentTime;
        public FloatSetting DayLength;
        public BoolSetting Pause;

        public override void Override(BaseSettings settings)
        {
            
        }
    }
}
