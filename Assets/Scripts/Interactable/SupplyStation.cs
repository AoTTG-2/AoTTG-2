using UnityEngine;

[RequireComponent(typeof(Interactable))]
public sealed class SupplyStation : MonoBehaviour, IInteractable
{
    string IInteractable.DefaultIconPath => "ui/Minimap/Supply Station";

    public void OnInteracted(GameObject player)
    {
        var hero = player.GetComponent<Hero>();
        Debug.Assert(hero != null, "Interacted event did not send a player.");

        Debug.Assert(
            IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || hero.photonView.isMine,
            "Interactable should only act on local player.");

        hero.getSupply();
    }
}