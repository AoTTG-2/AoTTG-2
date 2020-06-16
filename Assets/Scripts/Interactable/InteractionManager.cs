using System.Collections.Generic;
using UnityEngine;

public sealed class InteractionManager : MonoBehaviour
{
    private static InteractionManager _instance;
    private readonly HashSet<Interactable> interactables = new HashSet<Interactable>();
    private GameObject player;

    public static IReadOnlyCollection<Interactable> Interactables => _instance.interactables;

    public static GameObject Player => _instance.player;

    public static void Register(Interactable interactable) =>
        _instance.interactables.Add(interactable);

    public static void Unregister(Interactable interactable) =>
        _instance.interactables.Remove(interactable);

    private static Interactable GetInteractable(GameObject gobj)
    {
        var interactable = gobj.GetComponentInParent<Interactable>();
        Debug.Assert(interactable != null, "Interactable expected in parent of GameObject with Interactable layer");
        return interactable;
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

    private void RegisterSingleton()
    {
        Debug.Assert(_instance == null || _instance == this, "There is more than one InteractionManager in the scene.");
        _instance = this;
    }
}