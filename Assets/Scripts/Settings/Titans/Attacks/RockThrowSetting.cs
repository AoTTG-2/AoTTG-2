using Assets.Scripts.Gamemode;

namespace Assets.Scripts.Settings.Titans.Attacks
{
    public class RockThrowSetting : AttackSetting
    {
        public bool? Enabled { get; set; }

        public RockThrowSetting() { }

        public RockThrowSetting(Difficulty difficulty)
        {
            Enabled = true;
            if (difficulty == Difficulty.Realism)
                Enabled = false;
        }
    }
}
