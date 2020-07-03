using UnityEngine;

public sealed class Interactable : MonoBehaviour
{
    public InteractedEvent Interacted = new InteractedEvent();

    [SerializeField, Tooltip("Text displayed on InteractionWheel.")]
    private string context = string.Empty;

    [SerializeField, Tooltip("Icon displayed on InteractionWheel button.")]
    private UnityEngine.Sprite icon;

    [SerializeField]
    private InteractionPriority priority = InteractionPriority.Default;

    public Observable<bool> Available { get; private set; } = new Observable<bool>(true);

    public string Context
    {
        get { return context; }
        set { context = value; }
    }

    public UnityEngine.Sprite Icon
    {
        get { return icon; }
        set { icon = value; }
    }

    public InteractionPriority Priority => priority;

    public void Interact(Hero hero)
    {
        Interacted.Invoke(hero);
    }

    private void OnDestroy()
    {
        // InteractionManager.OnTriggerExit is not called when this is destroyed.
        InteractionManager.Unregister(this);
    }
}