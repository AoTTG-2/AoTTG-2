using UnityEngine;

[RequireComponent(typeof(Interactable))]
public sealed class SupplyStation : MonoBehaviour, IInteractable
{
#if UNITY_EDITOR
    private Interactable interactable;
#endif

    string IInteractable.DefaultIconPath => "ui/Minimap/Supply Station";

    public void OnInteracted(GameObject player)
    {
        Debug.Log("OnInteracted");

        var hero = player.GetComponent<Hero>();
        Debug.Assert(hero != null, "Interacted event did not send a player.");

        Debug.Assert(
            IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || hero.photonView.isMine,
            "Interactable should only act on local player.");

        hero.getSupply();
    }

#if UNITY_EDITOR

    private void OnDestroy()
    {
        if (interactable)
            DestroyImmediate(interactable);
    }

    private void Reset()
    {
        interactable = GetComponent<Interactable>();
    }

#endif
}