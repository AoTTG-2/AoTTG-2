using Assets.Scripts.Events;
using Assets.Scripts.Room;

namespace Assets.Scripts.Services.Interface
{
    public interface ILevelService
    {
        /// <summary>
        /// Invoked when the scene or custom level was loaded. Always use this instead of OnLevelWasLoaded / SceneManager.sceneLoaded to remain compatible with custom maps
        /// </summary>
        event OnLevelLoaded OnLevelLoaded;
        void InvokeLevelLoaded(int scene, Level level);
    }
}
