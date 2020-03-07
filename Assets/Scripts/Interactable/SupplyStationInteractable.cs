using UnityEngine;

public class SupplyStationInteractable : Interactable
{
    private void Start () 
    {
        if (Minimap.instance != null)
        {
            Minimap.instance.TrackGameObjectOnMinimap(base.gameObject, Color.white, false, true, Minimap.IconStyle.SUPPLY);
        }
	}
	
    public override void Action(GameObject target)
    {
        var hero = target.GetComponent<Hero>();
        if (hero == null) return;

        var distance = Vector3.Distance(target.transform.position, base.transform.position);
        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || hero.isMine && distance < Radius)
        {
            hero.getSupply();
        }
    }
}
