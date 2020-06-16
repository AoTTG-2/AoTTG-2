using System;
using UnityEngine;
using UnityEngine.Events;

[ExecuteInEditMode]
public sealed class Interactable : MonoBehaviour
{
    public InteractedEvent Interacted;

#if UNITY_EDITOR
    private new CapsuleCollider collider;
#endif

    [SerializeField, Tooltip("Text displayed on InteractionWheel.")]
    private string context = string.Empty;

    [SerializeField, Tooltip("Icon displayed on InteractionWheel button.")]
    private UnityEngine.Sprite icon;

    public string Context => context;

    public UnityEngine.Sprite Icon
    {
        get { return icon; }
        set { icon = value; }
    }

    public void Interact(GameObject player)
    {
        Interacted.Invoke(player);
    }

    private void OnDestroy()
    {
        InteractionManager.Unregister(this);
    }

#if UNITY_EDITOR

    public void AddComponents()
    {
        collider = FindOrCreateCollider();
        var interactable = GetComponent<IInteractable>();
        Icon = Resources.Load<UnityEngine.Sprite>(interactable.DefaultIconPath);
        UnityEditor.Events.UnityEventTools.RemovePersistentListener<GameObject>(Interacted, interactable.OnInteracted);
        UnityEditor.Events.UnityEventTools.AddPersistentListener(Interacted, interactable.OnInteracted);
    }

    public void RemoveComponents()
    {
        if (collider)
            DestroyImmediate(collider.gameObject);
    }

    private CapsuleCollider FindOrCreateCollider()
    {
        var found = transform.Find("Interactable");
        if (found)
            return found.GetComponent<CapsuleCollider>();

        var interactable = new GameObject("Interactable")
        {
            layer = LayerMask.NameToLayer(Layer.Interactable)
        };

        interactable.transform.parent = transform;
        interactable.transform.localPosition = Vector3.zero;

        var collider = interactable.AddComponent<CapsuleCollider>();
        collider.radius = 7;
        collider.isTrigger = true;

        return collider;
    }

    private void Reset()
    {
        if (string.IsNullOrEmpty(Context))
            context = name;
    }

#endif

    [Serializable]
    public class InteractedEvent : UnityEvent<GameObject> { }
}