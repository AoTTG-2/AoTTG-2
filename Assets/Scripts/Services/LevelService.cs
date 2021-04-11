using Assets.Scripts.Events;
using Assets.Scripts.Services.Interface;

namespace Assets.Scripts.Services
{
    public class LevelService : ILevelService
    {
        public event OnLevelLoaded OnLevelLoaded;
        public void InvokeLevelLoaded()
        {
            OnLevelLoaded?.Invoke();
        }
    }
}
