using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class ByteReader
{
    private byte[] mBuffer;
    private int mOffset;

    public ByteReader(byte[] bytes)
    {
        this.mBuffer = bytes;
    }

    public ByteReader(TextAsset asset)
    {
        this.mBuffer = asset.bytes;
    }

    public Dictionary<string, string> ReadDictionary()
    {
        Dictionary<string, string> dictionary = new Dictionary<string, string>();
        char[] separator = new char[] { '=' };
        while (this.canRead)
        {
            string str = this.ReadLine();
            if (str == null)
            {
                return dictionary;
            }
            if (!str.StartsWith("//"))
            {
                string[] strArray = str.Split(separator, 2, StringSplitOptions.RemoveEmptyEntries);
                if (strArray.Length == 2)
                {
                    string str2 = strArray[0].Trim();
                    string str3 = strArray[1].Trim().Replace(@"\n", "\n");
                    dictionary[str2] = str3;
                }
            }
        }
        return dictionary;
    }

    public string ReadLine()
    {
        string str;
        int length = this.mBuffer.Length;
        while (this.mOffset < length)
        {
            if (this.mBuffer[this.mOffset] >= 0x20)
            {
                break;
            }
            this.mOffset++;
        }
        int mOffset = this.mOffset;
        if (mOffset >= length)
        {
            this.mOffset = length;
            return null;
        }
        while (mOffset < length)
        {
            switch (this.mBuffer[mOffset++])
            {
                case 10:
                case 13:
                    goto Label_005F;
            }
        }
        mOffset++;
    Label_005F:
        str = ReadLine(this.mBuffer, this.mOffset, (mOffset - this.mOffset) - 1);
        this.mOffset = mOffset;
        return str;
    }

    private static string ReadLine(byte[] buffer, int start, int count)
    {
        return Encoding.UTF8.GetString(buffer, start, count);
    }

    public bool canRead
    {
        get
        {
            return ((this.mBuffer != null) && (this.mOffset < this.mBuffer.Length));
        }
    }
}

