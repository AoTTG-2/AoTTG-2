using System;
using UnityEngine;

public class BTN_SAVE_CC : MonoBehaviour
{
    public GameObject manager;

    private void OnClick()
    {
        this.manager.GetComponent<CustomCharacterManager>().SaveData();
    }
}

