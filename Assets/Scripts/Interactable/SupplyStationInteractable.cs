using Assets.Scripts.Characters.Humans;
using UnityEngine;

/// <summary>
/// Supply Station interactable
/// </summary>
public class SupplyStationInteractable : Interactable
{

    public override void Action(GameObject target)
    {
        Hero hero = target.GetComponent<Hero>();
        if (hero == null) return;
        var distance = Vector3.Distance(target.transform.position, base.transform.position);
        if (hero.photonView.isMine && distance < Radius)
        {
            hero.GetSupply();
        }
    }
}
