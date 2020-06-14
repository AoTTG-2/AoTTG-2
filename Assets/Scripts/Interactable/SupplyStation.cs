using System.Collections;
using UnityEngine;

[ExecuteInEditMode]
public sealed class SupplyStation : MonoBehaviour
{
    private const string DefaultSupplyStationIcon = "ui/Minimap/Supply Station";
    private Interactable interactable;

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

    // TODO: Find a better way to add this listener. It currently has to wait for Interacted to initialize.
    private IEnumerator AddListener()
    {
        while (interactable.Interacted == null)
            yield return null;

        UnityEditor.Events.UnityEventTools.AddPersistentListener(interactable.Interacted, OnInteracted);
    }

    private void OnDestroy()
    {
        if (interactable)
            DestroyImmediate(interactable);
    }

    private void Reset()
    {
        interactable = GetComponent<Interactable>() ?? gameObject.AddComponent<Interactable>();
        interactable.Icon = Resources.Load<UnityEngine.Sprite>(DefaultSupplyStationIcon);
        StartCoroutine(AddListener());
    }

#endif
}