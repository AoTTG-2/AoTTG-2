using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed class InteractionManager : MonoBehaviour
{
    private static InteractionManager _instance;
    private readonly SortedSet<Interactable> interactables = new SortedSet<Interactable>(Comparer<Interactable>.Create((x, y) => -x.Priority.CompareTo(y.Priority)));
    private GameObject player;

    public static IEnumerable<Interactable> AvailableInteractables => _instance.interactables.Where(i => i.Available);

    public static GameObject Player => _instance.player;

    public static void Register(Interactable interactable) =>
        _instance?.interactables.Add(interactable);

    public static void Unregister(Interactable interactable) =>
        _instance?.interactables.Remove(interactable);

    private static Interactable[] GetInteractables(GameObject gobj)
    {
        return gobj.GetComponentsInParent<Interactable>(true);
    }

    private void Awake()
    {
        RegisterSingleton();
        player = gameObject;
    }

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.layer == LayerMask.NameToLayer(Layer.Interactable))
            foreach (var interactable in GetInteractables(coll.gameObject))
                Register(interactable);
    }

    private void OnTriggerExit(Collider coll)
    {
        if (coll.gameObject.layer == LayerMask.NameToLayer(Layer.Interactable))
            foreach (var interactable in GetInteractables(coll.gameObject))
                Unregister(interactable);
    }

    private void RegisterSingleton()
    {
        Debug.Assert(_instance == null || _instance == this, "There is more than one InteractionManager in the scene.");
        _instance = this;
    }
}