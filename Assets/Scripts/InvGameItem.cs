using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InvGameItem
{
    public int itemLevel;
    private InvBaseItem mBaseItem;
    [SerializeField]
    private int mBaseItemID;
    public Quality quality;

    public InvGameItem(int id)
    {
        this.quality = Quality.Sturdy;
        this.itemLevel = 1;
        this.mBaseItemID = id;
    }

    public InvGameItem(int id, InvBaseItem bi)
    {
        this.quality = Quality.Sturdy;
        this.itemLevel = 1;
        this.mBaseItemID = id;
        this.mBaseItem = bi;
    }

    public List<InvStat> CalculateStats()
    {
        List<InvStat> list = new List<InvStat>();
        if (this.baseItem != null)
        {
            float statMultiplier = this.statMultiplier;
            List<InvStat> stats = this.baseItem.stats;
            int num2 = 0;
            int count = stats.Count;
            while (num2 < count)
            {
                InvStat stat = stats[num2];
                int num4 = Mathf.RoundToInt(statMultiplier * stat.amount);
                if (num4 != 0)
                {
                    bool flag = false;
                    int num5 = 0;
                    int num6 = list.Count;
                    while (num5 < num6)
                    {
                        InvStat stat2 = list[num5];
                        if ((stat2.id == stat.id) && (stat2.modifier == stat.modifier))
                        {
                            stat2.amount += num4;
                            flag = true;
                            break;
                        }
                        num5++;
                    }
                    if (!flag)
                    {
                        InvStat item = new InvStat
                        {
                            id = stat.id,
                            amount = num4,
                            modifier = stat.modifier
                        };
                        list.Add(item);
                    }
                }
                num2++;
            }
            list.Sort(new Comparison<InvStat>(InvStat.CompareArmor));
        }
        return list;
    }


    public InvBaseItem baseItem
    {
        get
        {
            if (this.mBaseItem == null)
            {
                this.mBaseItem = InvDatabase.FindByID(this.baseItemID);
            }
            return this.mBaseItem;
        }
    }

    public int baseItemID
    {
        get
        {
            return this.mBaseItemID;
        }
    }

    public Color color
    {
        get
        {
            Color white = Color.white;
            switch (this.quality)
            {
                case Quality.Broken:
                    return new Color(0.4f, 0.2f, 0.2f);

                case Quality.Cursed:
                    return Color.red;

                case Quality.Damaged:
                    return new Color(0.4f, 0.4f, 0.4f);

                case Quality.Worn:
                    return new Color(0.7f, 0.7f, 0.7f);

                case Quality.Sturdy:
                    return new Color(1f, 1f, 1f);

                case Quality.Polished:
                    return NGUIMath.HexToColor(0xe0ffbeff);

                case Quality.Improved:
                    return NGUIMath.HexToColor(0x93d749ff);

                case Quality.Crafted:
                    return NGUIMath.HexToColor(0x4eff00ff);

                case Quality.Superior:
                    return NGUIMath.HexToColor(0xbaffff);

                case Quality.Enchanted:
                    return NGUIMath.HexToColor(0x7376fdff);

                case Quality.Epic:
                    return NGUIMath.HexToColor(0x9600ffff);

                case Quality.Legendary:
                    return NGUIMath.HexToColor(0xff9000ff);
            }
            return white;
        }
    }

    public string name
    {
        get
        {
            if (this.baseItem == null)
            {
                return null;
            }
            return (this.quality.ToString() + " " + this.baseItem.name);
        }
    }

    public float statMultiplier
    {
        get
        {
            float num = 0f;
            switch (this.quality)
            {
                case Quality.Broken:
                    num = 0f;
                    break;

                case Quality.Cursed:
                    num = -1f;
                    break;

                case Quality.Damaged:
                    num = 0.25f;
                    break;

                case Quality.Worn:
                    num = 0.9f;
                    break;

                case Quality.Sturdy:
                    num = 1f;
                    break;

                case Quality.Polished:
                    num = 1.1f;
                    break;

                case Quality.Improved:
                    num = 1.25f;
                    break;

                case Quality.Crafted:
                    num = 1.5f;
                    break;

                case Quality.Superior:
                    num = 1.75f;
                    break;

                case Quality.Enchanted:
                    num = 2f;
                    break;

                case Quality.Epic:
                    num = 2.5f;
                    break;

                case Quality.Legendary:
                    num = 3f;
                    break;
            }
            float from = ((float) this.itemLevel) / 50f;
            return (num * Mathf.Lerp(from, from * from, 0.5f));
        }
    }

    public enum Quality
    {
        Broken,
        Cursed,
        Damaged,
        Worn,
        Sturdy,
        Polished,
        Improved,
        Crafted,
        Superior,
        Enchanted,
        Epic,
        Legendary,
        _LastDoNotUse
    }
}