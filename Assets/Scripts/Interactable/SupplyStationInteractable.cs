using UnityEngine;

public class SupplyStationInteractable : Interactable
{
    private void Start () 
    {

	}
	
    public override void Action(GameObject target)
    {

        Hero hero = target.GetComponent<Hero>();
        if (hero == null) return;
        var distance = Vector3.Distance(target.transform.position, base.transform.position);
        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || hero.photonView.isMine && distance < Radius)
        {
            hero.getSupply();
        }
    }
}
