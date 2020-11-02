using Assets.Scripts.Characters;
using UnityEngine;

namespace Assets.Scripts.Events
{
    //TODO: Rather use the gameobject
    public delegate void OnCheckpointArrived(Vector3 checkpoint, Entity arriver);
}
