using Assets.Scripts.Gamemode;

namespace Assets.Scripts.Settings
{
    public interface IGameSettingService
    {
        void Save(GameSettings settings);
        GameSettings Load(string custom);
        GameSettings Load(Difficulty difficulty);
    }
}
