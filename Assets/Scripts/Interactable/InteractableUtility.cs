#if UNITY_EDITOR

using UnityEditor.Events;

#endif

using UnityEngine;
using UnityEngine.Events;

public static class InteractableUtility
{
    private const string ColliderName = "Collider";

    /// <summary>
    /// Adds <paramref name="action"/> as a subscriber to <paramref name="event"/> in the editor.
    /// </summary>
    public static void RegisterUnique(InteractedEvent @event, UnityAction<Hero> action)
    {
#if UNITY_EDITOR
        UnityEventTools.RemovePersistentListener(@event, action);
        UnityEventTools.AddPersistentListener(@event, action);
#endif
    }

    public static void SetDefaults(this Interactable that, string context, UnityEngine.Sprite icon, params UnityAction<Hero>[] actions)
    {
        that.Context = context;

        that.Icon = icon;

        foreach (var action in actions)
            RegisterUnique(that.Interacted, action);
    }

    public static void SetDefaults(this Interactable that, string context, string iconPath, params UnityAction<Hero>[] actions) =>
        that.SetDefaults(context, Resources.Load<UnityEngine.Sprite>(iconPath), actions);

    public static void TryCreateCollider(this Interactable that)
    {
        Transform _;
        if (!that.transform.TryFindChild(ColliderName, out _))
            CreateCollider(that.transform);
    }

    public static void TryDestroyCollider(this Interactable that)
    {
        Transform collider;
        if (that.transform.TryFindChild(ColliderName, out collider))
            Object.DestroyImmediate(collider.gameObject);
    }

    private static void CreateCollider(Transform parent)
    {
        var gobj = new GameObject(ColliderName)
        {
            layer = LayerMask.NameToLayer(Layer.Interactable)
        };

        gobj.transform.SetParent(parent, false);
        
        var collider = gobj.AddComponent<CapsuleCollider>();
        collider.radius = 7;
        collider.isTrigger = true;
    }
}