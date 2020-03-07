using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    private CapsuleCollider collider;
    public int Radius = 7;

    void Awake()
    {
		var interactableObject = new GameObject("Interactable");
        interactableObject.layer = LayerMask.NameToLayer(Layer.Interactable);
        collider = interactableObject.AddComponent<CapsuleCollider>();
        collider.radius = Radius;
        collider.isTrigger = true;
        interactableObject.transform.parent = transform;
        interactableObject.transform.localPosition = new Vector3();
    }

    public abstract void Action(GameObject target);
}
