using Assets.Scripts.Settings.Types;
using UnityEngine;

namespace Assets.Scripts.Settings.Game
{
    [CreateAssetMenu(fileName = "Custom", menuName = "Settings/Time", order = 2)]
    public class TimeSettings : BaseSettings
    {
        public FloatSetting CurrentTime;
        public FloatSetting DayLength;
        public BoolSetting Pause;

        public override void Override(BaseSettings settings)
        {
            if (!(settings is TimeSettings time)) return;
            if (time.CurrentTime.HasValue) CurrentTime.Value = time.CurrentTime.Value;
            if (time.DayLength.HasValue) DayLength.Value = time.DayLength.Value;
            if (time.Pause.HasValue) Pause.Value = time.Pause.Value;
        }
    }
}
