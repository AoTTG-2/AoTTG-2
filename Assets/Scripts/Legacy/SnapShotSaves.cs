using System;
using UnityEngine;

[Obsolete("Logic regarding the snapshots. This has been mostly disabled, but needs to be re-enabled again")]
public class SnapShotSaves
{
    private static int currentIndex;
    private static int[] dmg;
    private static Texture2D[] img;
    private static int index;
    private static bool inited;
    private static int maxIndex;

    public static void addIMG(Texture2D t, int d)
    {
        init();
        img[index] = t;
        dmg[index] = d;
        currentIndex = index;
        index++;
        if (index >= img.Length)
        {
            index = 0;
        }
        maxIndex = Mathf.Max(index, maxIndex);
    }

    public static int getCurrentDMG()
    {
        if (maxIndex == 0)
        {
            return 0;
        }
        return dmg[currentIndex];
    }

    public static Texture2D getCurrentIMG()
    {
        if (maxIndex == 0)
        {
            return null;
        }
        return img[currentIndex];
    }

    public static int getCurrentIndex()
    {
        return currentIndex;
    }

    public static int getLength()
    {
        return maxIndex;
    }

    public static int getMaxIndex()
    {
        return maxIndex;
    }

    public static Texture2D GetNextIMG()
    {
        currentIndex++;
        if (currentIndex >= maxIndex)
        {
            currentIndex = 0;
        }
        return getCurrentIMG();
    }

    public static Texture2D GetPrevIMG()
    {
        currentIndex--;
        if (currentIndex < 0)
        {
            currentIndex = maxIndex - 1;
        }
        return getCurrentIMG();
    }

    public static void init()
    {
        if (!inited)
        {
            inited = true;
            index = 0;
            maxIndex = 0;
            img = new Texture2D[0x63];
            dmg = new int[0x63];
        }
    }
}

