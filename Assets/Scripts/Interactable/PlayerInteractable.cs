using Assets.Scripts.Constants;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerInteractable : Interactable
{

    public List<Interactable> Collisions { get; set; } = new List<Interactable>();

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.layer == (int)Layers.Interactable)
        {
            var interactable = col.gameObject.GetComponentInParent<Interactable>();
            if (Collisions.Any(x => x == interactable)) return;
            Collisions.Add(interactable);
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.layer == (int)Layers.Interactable)
        {
            Collisions.Remove(col.gameObject.GetComponentInParent<Interactable>());
        }
    }

    public override void Action(GameObject target)
    {
        //throw new System.NotImplementedException();
    }
}
