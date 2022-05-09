using Assets.Scripts.Events;
using Assets.Scripts.Room;
using Assets.Scripts.Services.Interface;
using UnityEngine;

namespace Assets.Scripts.Services
{
    public class LevelService : ILevelService
    {
        public event OnLevelLoaded OnLevelLoaded;
        public void InvokeLevelLoaded(int scene, Level level)
        {
            Debug.Log("OnLevelLoaded()");
            OnLevelLoaded?.Invoke(scene, level);
        }
    }
}
