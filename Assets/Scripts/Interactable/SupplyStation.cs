using UnityEngine;

[RequireComponent(typeof(Interactable))]
public sealed class SupplyStation : MonoBehaviour
{
    public void Interact(Hero player)
    {
        var hero = player.GetComponent<Hero>();
        Debug.Assert(hero != null, "Interacted event did not send a player.");

        Debug.Assert(
            IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || hero.photonView.isMine,
            "Interactable should only act on local player.");

        hero.getSupply();
    }

    private void Reset()
    {
        GetComponent<Interactable>().SetDefaults("Resupply", "ui/Minimap/Supply Station", Interact);
    }
}