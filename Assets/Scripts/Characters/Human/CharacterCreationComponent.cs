using System;
using UnityEngine;

public class CharacterCreationComponent : MonoBehaviour
{
    public GameObject manager;
    public CreatePart part;

    public void nextOption()
    {
        this.manager.GetComponent<CustomCharacterManager>().nextOption(this.part);
    }

    public void prevOption()
    {
        this.manager.GetComponent<CustomCharacterManager>().prevOption(this.part);
    }
}

