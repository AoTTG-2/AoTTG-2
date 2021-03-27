using Assets.Scripts.Gamemode;

namespace Assets.Scripts.Settings
{
    public class GlobalSettings
    {
        public float? Gravity { get; set; }

        public GlobalSettings() { }

        public GlobalSettings(Difficulty difficulty)
        {
            Gravity = 1.0f;
        }

    }
}