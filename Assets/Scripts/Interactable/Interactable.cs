using Assets.Scripts.Constants;
using UnityEngine;

/// <summary>
/// Abstract class for the "Interactables" functionality. Allows players to interact with various objects in the world
/// </summary>
public abstract class Interactable : MonoBehaviour
{
    private CapsuleCollider collider;
    public int Radius = 7;

    //Text displayed on Interactable wheel.
    public string Context = "";
    //Icon displayed on Interactable wheel button.
    public UnityEngine.Sprite Icon;

    void Awake()
    {
		var interactableObject = new GameObject("Interactable");
        interactableObject.layer = (int)Layers.Interactable;
        collider = interactableObject.AddComponent<CapsuleCollider>();
        collider.radius = Radius;
        collider.isTrigger = true;
        interactableObject.transform.parent = transform;
        interactableObject.transform.localPosition = new Vector3();
        if(string.IsNullOrEmpty(Context))
            Context = name;
    }

    public abstract void Action(GameObject target);
}
