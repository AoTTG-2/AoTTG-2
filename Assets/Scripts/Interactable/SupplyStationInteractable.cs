using UnityEngine;

public sealed class SupplyStationInteractable : Interactable
{
    public override void Action(GameObject target)
    {
        var hero = target.GetComponent<Hero>();
        if (hero == null) return;
        var distance = Vector3.Distance(target.transform.position, base.transform.position);
        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || hero.photonView.isMine && distance < Radius)
            hero.getSupply();
    }
}