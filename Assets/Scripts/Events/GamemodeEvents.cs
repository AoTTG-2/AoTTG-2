using Assets.Scripts.Characters;
using Assets.Scripts.Room;
using UnityEngine;

namespace Assets.Scripts.Events
{
    //TODO: Rather use the gameobject
    public delegate void OnCheckpointArrived(Vector3 checkpoint, Entity arriver);
    /// <summary>
    /// Event is fired whenever a Level has been loaded. Use this method over OnLevelWasLoaded or SceneManager.OnLoaded as it will only trigger after a map, including CustomMap has been loaded
    /// </summary>
    public delegate void OnLevelLoaded(int scene, Level level);
}
