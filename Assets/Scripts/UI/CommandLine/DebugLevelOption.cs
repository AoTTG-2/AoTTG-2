using UnityEngine;
using System.Collections;
using Assets.Scripts.UI;
using System.Collections.Generic;

public class DebugLevelOption
{
    private bool enabled;
    public DebugLevel DebugLevel;
    public bool NeedRefresh = false;
    public bool Enabled
    {
        get
        {
            return enabled;
        }
        set
        {
            if (enabled!=value) NeedRefresh = true;

            enabled = value;
        }
    }

    public static List<DebugLevelOption> DebugLevelOptions = new List<DebugLevelOption>()
    {
        new DebugLevelOption(true, DebugLevel.Alert),
        new DebugLevelOption(true, DebugLevel.Critical),
        new DebugLevelOption(true, DebugLevel.Debug),
        new DebugLevelOption(true, DebugLevel.Default),
        new DebugLevelOption(true, DebugLevel.Emergency),
        new DebugLevelOption(true, DebugLevel.Error),
        new DebugLevelOption(true, DebugLevel.Info),
        new DebugLevelOption(true, DebugLevel.Notification),
        new DebugLevelOption(true, DebugLevel.Warning)
    };

    public DebugLevelOption(bool isEnabled, DebugLevel debugLevel)
    {
        enabled = isEnabled;
        DebugLevel = debugLevel;
    }
}
