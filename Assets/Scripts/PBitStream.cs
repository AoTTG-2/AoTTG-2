using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class PBitStream
{
    public int currentByte;
    public List<byte> streamBytes;
    private int totalBits;

    public PBitStream()
    {
        this.streamBytes = new List<byte>(1);
    }

    public PBitStream(int bitCount)
    {
        this.streamBytes = new List<byte>(BytesForBits(bitCount));
    }

    public PBitStream(IEnumerable<byte> bytes, int bitCount)
    {
        this.streamBytes = new List<byte>(bytes);
        this.BitCount = bitCount;
    }

    public void Add(bool val)
    {
        int num = this.totalBits / 8;
        if ((num > (this.streamBytes.Count - 1)) || (this.totalBits == 0))
        {
            this.streamBytes.Add(0);
        }
        if (val)
        {
            int currentByteBits = 7 - (this.totalBits % 8);
            this.streamBytes[num] |= (byte)(1 << currentByteBits);
        }
        this.totalBits++;
    }

    public static int BytesForBits(int bitCount)
    {
        if (bitCount <= 0)
        {
            return 0;
        }
        return (((bitCount - 1) / 8) + 1);
    }

    public bool Get(int bitIndex)
    {
        int num = bitIndex / 8;
        int num2 = 7 - (bitIndex % 8);
        return ((this.streamBytes[num] & ((byte)(((int)1) << num2))) > 0);
    }

    public bool GetNext()
    {
        int num;
        if (this.Position > this.totalBits)
        {
            throw new Exception("End of PBitStream reached. Can't read more.");
        }
        this.Position = (num = this.Position) + 1;
        return this.Get(num);
    }

    public void Set(int bitIndex, bool value)
    {
        int byteIndex = bitIndex / 8;
        int bitInByIndex = 7 - (bitIndex % 8);
        this.streamBytes[byteIndex] |= (byte)(1 << bitInByIndex);
    }

    public byte[] ToBytes()
    {
        return this.streamBytes.ToArray();
    }

    public int BitCount
    {
        get
        {
            return this.totalBits;
        }
        private set
        {
            this.totalBits = value;
        }
    }

    public int ByteCount
    {
        get
        {
            return BytesForBits(this.totalBits);
        }
    }

    public int Position { get; set; }
}

