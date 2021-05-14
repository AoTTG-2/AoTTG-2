using Assets.Scripts.Events;
using Assets.Scripts.Room;

namespace Assets.Scripts.Services.Interface
{
    public interface ILevelService
    {
        event OnLevelLoaded OnLevelLoaded;
        void InvokeLevelLoaded(int scene, Level level);
    }
}
