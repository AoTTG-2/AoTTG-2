using System;
using System.Collections.Generic;
using UnityEngine;

public class PhotonStream
{
    private byte currentItem;
    internal List<object> data;
    private bool write;

    public PhotonStream(bool write, object[] incomingData)
    {
        this.write = write;
        if (incomingData == null)
        {
            this.data = new List<object>();
        }
        else
        {
            this.data = new List<object>(incomingData);
        }
    }

    public object ReceiveNext()
    {
        if (this.write)
        {
            Debug.LogError("Error: you cannot read this stream that you are writing!");
            return null;
        }
        object obj2 = this.data[this.currentItem];
        this.currentItem = (byte) (this.currentItem + 1);
        return obj2;
    }

    public void SendNext(object obj)
    {
        if (!this.write)
        {
            Debug.LogError("Error: you cannot write/send to this stream that you are reading!");
        }
        else
        {
            this.data.Add(obj);
        }
    }

    public void Serialize(ref PhotonPlayer obj)
    {
        if (this.write)
        {
            this.data.Add(obj);
        }
        else if (this.data.Count > this.currentItem)
        {
            obj = (PhotonPlayer) this.data[this.currentItem];
            this.currentItem = (byte) (this.currentItem + 1);
        }
    }

    public void Serialize(ref bool myBool)
    {
        if (this.write)
        {
            this.data.Add((bool) myBool);
        }
        else if (this.data.Count > this.currentItem)
        {
            myBool = (bool) this.data[this.currentItem];
            this.currentItem = (byte) (this.currentItem + 1);
        }
    }

    public void Serialize(ref char value)
    {
        if (this.write)
        {
            this.data.Add((char) value);
        }
        else if (this.data.Count > this.currentItem)
        {
            value = (char) this.data[this.currentItem];
            this.currentItem = (byte) (this.currentItem + 1);
        }
    }

    public void Serialize(ref short value)
    {
        if (this.write)
        {
            this.data.Add((short) value);
        }
        else if (this.data.Count > this.currentItem)
        {
            value = (short) this.data[this.currentItem];
            this.currentItem = (byte) (this.currentItem + 1);
        }
    }

    public void Serialize(ref int myInt)
    {
        if (this.write)
        {
            this.data.Add((int) myInt);
        }
        else if (this.data.Count > this.currentItem)
        {
            myInt = (int) this.data[this.currentItem];
            this.currentItem = (byte) (this.currentItem + 1);
        }
    }

    public void Serialize(ref float obj)
    {
        if (this.write)
        {
            this.data.Add((float) obj);
        }
        else if (this.data.Count > this.currentItem)
        {
            obj = (float) this.data[this.currentItem];
            this.currentItem = (byte) (this.currentItem + 1);
        }
    }

    public void Serialize(ref string value)
    {
        if (this.write)
        {
            this.data.Add(value);
        }
        else if (this.data.Count > this.currentItem)
        {
            value = (string) this.data[this.currentItem];
            this.currentItem = (byte) (this.currentItem + 1);
        }
    }

    public void Serialize(ref Quaternion obj)
    {
        if (this.write)
        {
            this.data.Add((Quaternion) obj);
        }
        else if (this.data.Count > this.currentItem)
        {
            obj = (Quaternion) this.data[this.currentItem];
            this.currentItem = (byte) (this.currentItem + 1);
        }
    }

    public void Serialize(ref Vector2 obj)
    {
        if (this.write)
        {
            this.data.Add((Vector2) obj);
        }
        else if (this.data.Count > this.currentItem)
        {
            obj = (Vector2) this.data[this.currentItem];
            this.currentItem = (byte) (this.currentItem + 1);
        }
    }

    public void Serialize(ref Vector3 obj)
    {
        if (this.write)
        {
            this.data.Add((Vector3) obj);
        }
        else if (this.data.Count > this.currentItem)
        {
            obj = (Vector3) this.data[this.currentItem];
            this.currentItem = (byte) (this.currentItem + 1);
        }
    }

    public object[] ToArray()
    {
        return this.data.ToArray();
    }

    public int Count
    {
        get
        {
            return this.data.Count;
        }
    }

    public bool isReading
    {
        get
        {
            return !this.write;
        }
    }

    public bool isWriting
    {
        get
        {
            return this.write;
        }
    }
}

