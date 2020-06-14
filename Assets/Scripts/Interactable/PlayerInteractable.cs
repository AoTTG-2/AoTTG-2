using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed class PlayerInteractable : Interactable
{
    public List<Interactable> Collisions { get; set; } = new List<Interactable>();

    public override void Action(GameObject target)
    {
        //throw new System.NotImplementedException();
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer(Layer.Interactable))
        {
            var interactable = col.gameObject.GetComponentInParent<Interactable>();
            if (Collisions.Any(x => x == interactable)) return;
            Collisions.Add(interactable);
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer(Layer.Interactable))
            Collisions.Remove(col.gameObject.GetComponentInParent<Interactable>());
    }
}