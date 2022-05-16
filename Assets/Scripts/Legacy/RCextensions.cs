using System;
using UnityEngine;

[Obsolete("Various extension methods which were created for the RC mod, however these are very poorly optimized and should not be used.")]
public static class RCextensions
{
    public static void Add<T>(ref T[] source, T value)
    {
        T[] localArray = new T[source.Length + 1];
        for (int i = 0; i < source.Length; i++)
        {
            localArray[i] = source[i];
        }
        localArray[localArray.Length - 1] = value;
        source = localArray;
    }

    public static string hexColor(this string text)
    {
        if (text.Contains("]"))
        {
            text = text.Replace("]", ">");
        }
        bool flag2 = false;
        while (text.Contains("[") && !flag2)
        {
            int index = text.IndexOf("[");
            if (text.Length >= (index + 7))
            {
                string str = text.Substring(index + 1, 6);
                text = text.Remove(index, 7).Insert(index, "<color=#" + str);
                int length = text.Length;
                if (text.Contains("["))
                {
                    length = text.IndexOf("[");
                }
                text = text.Insert(length, "</color>");
            }
            else
            {
                flag2 = true;
            }
        }
        if (flag2)
        {
            return string.Empty;
        }
        return text;
    }

    public static bool isLowestID(this PhotonPlayer player)
    {
        foreach (PhotonPlayer player2 in PhotonNetwork.playerList)
        {
            if (player2.ID < player.ID)
            {
                return false;
            }
        }
        return true;
    }

    public static Texture2D loadimage(WWW link, bool mipmap, int size)
    {
        Texture2D tex = new Texture2D(4, 4, TextureFormat.DXT1, mipmap);
        if (link.size >= size)
        {
            return tex;
        }
        Texture2D texture = link.texture;
        int width = texture.width;
        int height = texture.height;
        int num3 = 0;
        if ((width < 4) || ((width & (width - 1)) != 0))
        {
            num3 = 4;
            width = Math.Min(width, 0x3ff);
            while (num3 < width)
            {
                num3 *= 2;
            }
        }
        else if ((height < 4) || ((height & (height - 1)) != 0))
        {
            num3 = 4;
            height = Math.Min(height, 0x3ff);
            while (num3 < height)
            {
                num3 *= 2;
            }
        }
        if (num3 == 0)
        {
            if (mipmap)
            {
                try
                {
                    link.LoadImageIntoTexture(tex);
                }
                catch
                {
                    tex = new Texture2D(4, 4, TextureFormat.DXT1, false);
                    link.LoadImageIntoTexture(tex);
                }
                return tex;
            }
            link.LoadImageIntoTexture(tex);
            return tex;
        }
        if (num3 < 4)
        {
            return tex;
        }
        Texture2D textured3 = new Texture2D(4, 4, TextureFormat.DXT1, false);
        link.LoadImageIntoTexture(textured3);
        if (mipmap)
        {
            try
            {
                textured3.Resize(num3, num3, TextureFormat.DXT1, mipmap);
            }
            catch
            {
                textured3.Resize(num3, num3, TextureFormat.DXT1, false);
            }
        }
        else
        {
            textured3.Resize(num3, num3, TextureFormat.DXT1, mipmap);
        }
        textured3.Apply();
        return textured3;
    }

    public static void RemoveAt<T>(ref T[] source, int index)
    {
        if (source.Length == 1)
        {
            source = new T[0];
        }
        else if (source.Length > 1)
        {
            T[] localArray = new T[source.Length - 1];
            int num = 0;
            int num2 = 0;
            while (num < source.Length)
            {
                if (num != index)
                {
                    localArray[num2] = source[num];
                    num2++;
                }
                num++;
            }
            source = localArray;
        }
    }

    public static bool returnBoolFromObject(object obj)
    {
        return (((obj != null) && (obj is bool)) && ((bool) obj));
    }

    public static float returnFloatFromObject(object obj)
    {
        if ((obj != null) && (obj is float))
        {
            return (float) obj;
        }
        return 0f;
    }

    public static int returnIntFromObject(object obj)
    {
        if ((obj != null) && (obj is int))
        {
            return (int) obj;
        }
        return 0;
    }

    public static string returnStringFromObject(object obj)
    {
        if (obj != null)
        {
            string str = obj as string;
            if (str != null)
            {
                return str;
            }
        }
        return string.Empty;
    }

    //Converter for PlayerPrefs
    public static int boolToInt(bool val)
    {
        if (val)
            return 1;
        else
            return 0;
    }

    public static bool intToBool(int val)
    {
        if (val != 0)
            return true;
        else
            return false;
    }
}

