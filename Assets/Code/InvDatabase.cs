using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode, AddComponentMenu("NGUI/Examples/Item Database")]
public class InvDatabase : MonoBehaviour
{
    public int databaseID;
    public UIAtlas iconAtlas;
    public List<InvBaseItem> items = new List<InvBaseItem>();
    private static bool mIsDirty = true;
    private static InvDatabase[] mList;

    public static InvBaseItem FindByID(int id32)
    {
        InvDatabase database = GetDatabase(id32 >> 0x10);
        return ((database == null) ? null : database.GetItem(id32 & 0xffff));
    }

    public static InvBaseItem FindByName(string exact)
    {
        int index = 0;
        int length = list.Length;
        while (index < length)
        {
            InvDatabase database = list[index];
            int num3 = 0;
            int count = database.items.Count;
            while (num3 < count)
            {
                InvBaseItem item = database.items[num3];
                if (item.name == exact)
                {
                    return item;
                }
                num3++;
            }
            index++;
        }
        return null;
    }

    public static int FindItemID(InvBaseItem item)
    {
        int index = 0;
        int length = list.Length;
        while (index < length)
        {
            InvDatabase database = list[index];
            if (database.items.Contains(item))
            {
                return ((database.databaseID << 0x10) | item.id16);
            }
            index++;
        }
        return -1;
    }

    private static InvDatabase GetDatabase(int dbID)
    {
        int index = 0;
        int length = list.Length;
        while (index < length)
        {
            InvDatabase database = list[index];
            if (database.databaseID == dbID)
            {
                return database;
            }
            index++;
        }
        return null;
    }

    private InvBaseItem GetItem(int id16)
    {
        int num = 0;
        int count = this.items.Count;
        while (num < count)
        {
            InvBaseItem item = this.items[num];
            if (item.id16 == id16)
            {
                return item;
            }
            num++;
        }
        return null;
    }

    private void OnDisable()
    {
        mIsDirty = true;
    }

    private void OnEnable()
    {
        mIsDirty = true;
    }

    public static InvDatabase[] list
    {
        get
        {
            if (mIsDirty)
            {
                mIsDirty = false;
                mList = NGUITools.FindActive<InvDatabase>();
            }
            return mList;
        }
    }
}

