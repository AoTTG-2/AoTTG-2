using System;
using UnityEngine;

[Obsolete("Will be replaced in Issue #34")]
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

