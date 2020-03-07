using System;
using UnityEngine;

public class CharacterStatComponent : MonoBehaviour
{
    public GameObject manager;
    public CreateStat type;

    public void nextOption()
    {
        this.manager.GetComponent<CustomCharacterManager>().nextStatOption(this.type);
    }

    public void prevOption()
    {
        this.manager.GetComponent<CustomCharacterManager>().prevStatOption(this.type);
    }
}

