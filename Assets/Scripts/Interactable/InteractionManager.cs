using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed class InteractionManager : MonoBehaviour
{
    private static InteractionManager _instance;
    private readonly SortedSet<Interactable> interactables = new SortedSet<Interactable>(Comparer<Interactable>.Create((x, y) => -x.Priority.CompareTo(y.Priority)));
    private GameObject player;

    public delegate void AvailableInteractablesChangedHandler(IEnumerable<Interactable> availableInteractables);

    public static event AvailableInteractablesChangedHandler AvailableInteractablesChanged;

    public static IEnumerable<Interactable> AvailableInteractables =>
        _instance.interactables.Where(i => i.Available);

    public static GameObject Player => _instance.player;

    public static void Register(Interactable interactable)
    {
        if (_instance.interactables.Contains(interactable))
            return;

        _instance?.interactables.Add(interactable);
        if (interactable.Available)
            InvokeAvailableInteractablesChanged();

        interactable.Available.ValueChanged += OnAvailabilityChanged;
    }

    public static void Unregister(Interactable interactable)
    {
        if (!_instance.interactables.Contains(interactable))
            return;
        
        _instance?.interactables.Remove(interactable);
        if (interactable.Available)
            InvokeAvailableInteractablesChanged();

        interactable.Available.ValueChanged -= OnAvailabilityChanged;
    }

    private static Interactable[] GetInteractables(GameObject gobj) =>
        gobj.GetComponentsInParent<Interactable>(true);

    private static void InvokeAvailableInteractablesChanged() =>
        AvailableInteractablesChanged?.Invoke(AvailableInteractables);

    private static void OnAvailabilityChanged(bool available) =>
        InvokeAvailableInteractablesChanged();

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