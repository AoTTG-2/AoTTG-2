using UnityEngine;

public class SupplyStationInteractable : Interactable
{
    private void Start () 
    {
        if (Minimap.instance != null)
        {
            Minimap.instance.TrackGameObjectOnMinimap(base.gameObject, Color.white, false, true, Minimap.IconStyle.SUPPLY);
        }
        base.Icon = Resources.Load<UnityEngine.Sprite>("ui/Minimap/Supply Station");
	}
	
    public override void Action(GameObject target)
    {

        Hero hero = target.GetComponent<Hero>();
        if (hero == null) return;
        var distance = Vector3.Distance(target.transform.position, base.transform.position);
        if (hero.photonView.isMine && distance < Radius)
        {
            hero.getSupply();
        }
    }
}
