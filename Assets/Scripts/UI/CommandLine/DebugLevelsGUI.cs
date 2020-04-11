using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.UI;

public class DebugLevelsGUI
{
    public static void OnGUI()
    {
        GUI.backgroundColor = Color.green;
        foreach(DebugLevelOption option in DebugLevelOption.DebugLevelOptions)
        {
            option.Enabled = GUILayout.Toggle(option.Enabled, option.DebugLevel.ToString());
            if (option.NeedRefresh)
            {
                EMCli.RefreshLayout();
            }
        }
    }
}
