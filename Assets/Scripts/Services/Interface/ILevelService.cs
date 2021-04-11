using Assets.Scripts.Events;

namespace Assets.Scripts.Services.Interface
{
    public interface ILevelService
    {
        event OnLevelLoaded OnLevelLoaded;
        void InvokeLevelLoaded();
    }
}
