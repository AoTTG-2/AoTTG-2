using System;
using UnityEngine;

public class CheckBoxCostume : MonoBehaviour
{
    public static int costumeSet;
    public int set = 1;

    private void OnActivate(bool yes)
    {
        if (yes)
        {
            costumeSet = this.set;
        }
    }
}

