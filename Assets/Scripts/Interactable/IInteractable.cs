using UnityEngine;

internal interface IInteractable
{
    string DefaultIconPath { get; }

    void OnInteracted(GameObject player);
}