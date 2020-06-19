using System.Collections.Generic;
using UnityEngine;

public sealed class MoveGroundCannon : MonoBehaviour
{
    [SerializeField]
    private Interactable
        startMoveInteractable,
        stopMoveInteractable;

    private bool IsMoving
    {
        set
        {
            startMoveInteractable.Available.Value = !value;
            stopMoveInteractable.Available.Value = value;
        }
    }

    public void StartMoving(GameObject _)
    {
        IsMoving = true;
    }

    public void StopMoving(GameObject _)
    {
        IsMoving = false;
    }

    private void Awake()
    {
        IsMoving = false;
    }

    private void Reset()
    {
        var found = new List<Interactable>();
        GetComponents(found);
        for (var i = 0; i < 2 - found.Count; i++)
            found.Add(gameObject.AddComponent<Interactable>());

        startMoveInteractable = found[0];
        stopMoveInteractable = found[1];

        startMoveInteractable.SetDefaults("Start Moving", (UnityEngine.Sprite) null, StartMoving);
        stopMoveInteractable.SetDefaults("Stop Moving", (UnityEngine.Sprite) null, StopMoving);
    }
}