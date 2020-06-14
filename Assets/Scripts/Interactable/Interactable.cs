using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    [Tooltip("Text displayed on Interactable wheel.")]
    public string Context = "";

    [Tooltip("Icon displayed on Interactable wheel button.")]
    public UnityEngine.Sprite Icon;

    [SerializeField]
    private int radius = 7;

    protected int Radius => radius;

    public abstract void Action(GameObject target);

    // HACK: Private Unity messages in extendable classes is dangerous.
    // They will be silently overwritten if a deriving class implements the message.
    private void Awake()
    {
        var interactableObject = new GameObject("Interactable")
        {
            layer = LayerMask.NameToLayer(Layer.Interactable)
        };

        // TODO: Instantiate this with RequireComponent and set good defaults in Reset.
        var collider = interactableObject.AddComponent<CapsuleCollider>();
        collider.radius = Radius;
        collider.isTrigger = true;

        interactableObject.transform.parent = transform;
        interactableObject.transform.localPosition = new Vector3();

        if (string.IsNullOrEmpty(Context))
            Context = name;
    }
}