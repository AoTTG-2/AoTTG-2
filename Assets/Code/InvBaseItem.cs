using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InvBaseItem
{
    public GameObject attachment;
    public Color color = Color.white;
    public string description;
    public UIAtlas iconAtlas;
    public string iconName = string.Empty;
    public int id16;
    public int maxItemLevel = 50;
    public int minItemLevel = 1;
    public string name;
    public Slot slot;
    public List<InvStat> stats = new List<InvStat>();

    public enum Slot
    {
        None,
        Weapon,
        Shield,
        Body,
        Shoulders,
        Bracers,
        Boots,
        Trinket,
        _LastDoNotUse
    }
}

