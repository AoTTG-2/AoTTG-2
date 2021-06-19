using Assets.Scripts.Gamemode;

namespace Assets.Scripts.Settings.Gamemodes
{
    public class TrainingSettings : GamemodeSettings
    {
        public TrainingSettings()
        {
            GamemodeType = GamemodeType.Training;
        }
        public TrainingSettings(Difficulty difficulty) : base(difficulty)
        {
            GamemodeType = GamemodeType.Training;
        }
    }
}
