using System.Collections.Generic;
using UnityEngine;

public sealed class InteractionManager : MonoBehaviour
{
    private readonly HashSet<Interactable> interactables = new HashSet<Interactable>();
    private GameObject player;

    public static IReadOnlyCollection<Interactable> Interactables => _instance.interactables;

    public static GameObject Player => _instance.player;

    private static InteractionManager _instance;

    private void Register(Interactable interactable) =>
        interactables.Add(interactable);

    private void Unregister(Interactable interactable) =>
        interactables.Remove(interactable);

    private void RegisterSingleton()
    {
        Debug.Assert(_instance == null || _instance == this, "There is more than one InteractionManager in the scene.");
        _instance = this;
    }

    private void Awake()
    {
        RegisterSingleton();
        player = gameObject;
    }

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.layer == LayerMask.NameToLayer(Layer.Interactable))
            Register(GetInteractable(coll.gameObject));
    }

    private void OnTriggerExit(Collider coll)
    {
        if (coll.gameObject.layer == LayerMask.NameToLayer(Layer.Interactable))
            Unregister(GetInteractable(coll.gameObject));
    }

    private static Interactable GetInteractable(GameObject gobj)
    {
        var interactable = gobj.GetComponentInParent<Interactable>();
        Debug.Assert(interactable != null, "Interactable expected in parent of GameObject with Interactable layer");
        return interactable;
    }
}