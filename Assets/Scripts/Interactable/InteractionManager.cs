using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed class InteractionManager : MonoBehaviour
{
    private static readonly IComparer<Interactable> Comparer = Comparer<Interactable>.Create((x, y) => -x.Priority.CompareTo(y.Priority));
    private static readonly List<Interactable> Interactables = new List<Interactable>();
    private static InteractionManager _instance;

    public delegate void AvailableInteractablesChangedHandler(IEnumerable<Interactable> availableInteractables);

    public static event AvailableInteractablesChangedHandler AvailableInteractablesChanged;

    public static IEnumerable<Interactable> AvailableInteractables =>
        Interactables.Where(i => i.Available);

    public static Hero Player { get; private set; }

    public static void Register(Interactable interactable)
    {
        if (Interactables.Contains(interactable))
            return;

        Interactables.Add(interactable);
        Interactables.Sort(Comparer);
        if (interactable.Available)
            InvokeAvailableInteractablesChanged();

        interactable.Available.ValueChanged += OnAvailabilityChanged;
    }

    public static void Unregister(Interactable interactable)
    {
        if (!Interactables.Remove(interactable))
            return;

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
        Player = GetComponent<Hero>();
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