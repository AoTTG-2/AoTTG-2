using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class MoveGroundCannon : MonoBehaviour
{
    public const string InteractableName = "MoveInteractable";
    private const string MovePointName = "MovePoint";

    [SerializeField]
    private Interactable
        mountInteractable,
        startMoveInteractable,
        stopMoveInteractable;

    private Coroutine movement;

    [SerializeField]
    private Transform movePoint;

    private bool IsMoving
    {
        set
        {
            mountInteractable.Available.Value = !value;
            startMoveInteractable.Available.Value = !value;
            stopMoveInteractable.Available.Value = value;
        }
    }

    public void StartMoving(GameObject mover)
    {
        IsMoving = true;

        movement = StartCoroutine(Move(mover.GetComponent<Hero>()));
    }

    public void StopMoving(GameObject mover)
    {
        StopCoroutine(movement);
        movement = null;

        IsMoving = false;
    }

    private void Awake()
    {
        IsMoving = false;
    }

    private void FindMountInteractable()
    {
        Transform mountInteractableTransform;
        if (transform.parent.TryFindChild(UnmannedCannon.InteractableName, out mountInteractableTransform))
            mountInteractable = mountInteractableTransform.GetComponent<Interactable>();
        else
            Debug.LogWarning($"Could not find {UnmannedCannon.InteractableName} on {transform.parent.name}.\nUse the inspector of {nameof(UnmannedCannon)} to instantiate it.");
    }

    private void FindOrCreateMoveInteractable()
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

    private void FindOrCreateMovePoint()
    {
        if (!transform.TryFindChild(MovePointName, out movePoint))
            (movePoint = new GameObject(MovePointName).transform).SetParent(transform, false);
    }

    private IEnumerator Move(Hero mover)
    {
        while (true)
        {
            //mover.
            yield return null;
        }
    }

    private void Reset()
    {
        FindMountInteractable();
        FindOrCreateMoveInteractable();
        FindOrCreateMovePoint();
    }
}