using System;

public static class CLZF2
{
    public static readonly long[] HashTable = new long[HSIZE];
    public static readonly uint HLOG = 14;
    public static readonly uint HSIZE = 0x4000;
    public static readonly uint MAX_LIT = 0x20;
    public static readonly uint MAX_OFF = 0x2000;
    public static readonly uint MAX_REF = 0x108;

    /*public static byte[] Compress(byte[] inputBytes)
    {
        int num = inputBytes.Length * 2;
        byte[] output = new byte[num];
        int count = lzf_compress(inputBytes, ref output);
        while (count == 0)
        {
            num *= 2;
            output = new byte[num];
            count = lzf_compress(inputBytes, ref output);
        }
        byte[] dst = new byte[count];
        Buffer.BlockCopy(output, 0, dst, 0, count);
        return dst;
    }

    public static byte[] Decompress(byte[] inputBytes)
    {
        int num = inputBytes.Length * 2;
        byte[] output = new byte[num];
        int count = lzf_decompress(inputBytes, ref output);
        while (count == 0)
        {
            num *= 2;
            output = new byte[num];
            count = lzf_decompress(inputBytes, ref output);
        }
        byte[] dst = new byte[count];
        Buffer.BlockCopy(output, 0, dst, 0, count);
        return dst;
    }

    public static int lzf_compress(byte[] input, ref byte[] output)
    {
        int length = input.Length;
        int num2 = output.Length;
        Array.Clear(HashTable, 0, (int) HSIZE);
        uint index = 0;
        uint num4 = 0;
        uint num5 = (uint) ((input[0] << 8) | input[1]);
        int num6 = 0;
        while (index >= (length - 2))
        {
            long num9;
            if (index == length)
            {
                if (num6 != 0)
                {
                    if (((num4 + num6) + ((ulong) 1L)) >= num2)
                    {
                        return 0;
                    }
                    output[num4++] = (byte) (num6 - 1);
                    num6 = -num6;
                    do
                    {
                        output[num4++] = input[(int) ((IntPtr) (index + num6))];
                    }
                    while (++num6 != 0);
                }
                return (int) num4;
            }
        Label_003D:
            num6++;
            index++;
            if (num6 == MAX_LIT)
            {
                if (((num4 + 1) + MAX_LIT) >= num2)
                {
                    return 0;
                }
                output[num4++] = (byte) (MAX_LIT - 1);
                num6 = -num6;
                do
                {
                    output[num4++] = input[(int) ((IntPtr) (index + num6))];
                }
                while (++num6 != 0);
            }
            continue;
        Label_009C:
            num5 = (num5 << 8) | input[(int) ((IntPtr) (index + 2))];
            long num7 = ((num5 ^ (num5 << 5)) >> ((0x18 - HLOG) - (num5 * 5))) & (HSIZE - 1);
            long num8 = HashTable[(int) ((IntPtr) num7)];
            HashTable[(int) ((IntPtr) num7)] = index;
            if (((((num9 = (index - num8) - 1L) >= MAX_OFF) || ((index + 4) >= length)) || ((num8 <= 0L) || (input[(int) ((IntPtr) num8)] != input[index]))) || ((input[(int) ((IntPtr) (num8 + 1L))] != input[(int) ((IntPtr) (index + 1))]) || (input[(int) ((IntPtr) (num8 + 2L))] != input[(int) ((IntPtr) (index + 2))])))
            {
                goto Label_003D;
            }
            uint num10 = 2;
            uint num11 = (uint) ((length - index) - 2);
            num11 = (num11 <= MAX_REF) ? num11 : MAX_REF;
            if ((((num4 + num6) + ((ulong) 1L)) + ((ulong) 3L)) < num2)
            {
                goto Label_01B1;
            }
            return 0;
        Label_019D:
            if (input[(int) ((IntPtr) (num8 + num10))] != input[index + num10])
            {
                goto Label_01BD;
            }
        Label_01B1:
            num10++;
            if (num10 < num11)
            {
                goto Label_019D;
            }
        Label_01BD:
            if (num6 != 0)
            {
                output[num4++] = (byte) (num6 - 1);
                num6 = -num6;
                do
                {
                    output[num4++] = input[(int) ((IntPtr) (index + num6))];
                }
                while (++num6 != 0);
            }
            num10 -= 2;
            index++;
            if (num10 < 7)
            {
                output[num4++] = (byte) ((num9 >> 8) + (num10 << 5));
            }
            else
            {
                output[num4++] = (byte) ((num9 >> 8) + 0xe0L);
                output[num4++] = (byte) (num10 - 7);
            }
            output[num4++] = (byte) num9;
            index += num10 - 1;
            num5 = (uint) ((input[index] << 8) | input[(int) ((IntPtr) (index + 1))]);
            num5 = (num5 << 8) | input[(int) ((IntPtr) (index + 2))];
            HashTable[(int) ((IntPtr) (((num5 ^ (num5 << 5)) >> ((0x18 - HLOG) - (num5 * 5))) & (HSIZE - 1)))] = index;
            index++;
            num5 = (num5 << 8) | input[(int) ((IntPtr) (index + 2))];
            HashTable[(int) ((IntPtr) (((num5 ^ (num5 << 5)) >> ((0x18 - HLOG) - (num5 * 5))) & (HSIZE - 1)))] = index;
            index++;
        }
        goto Label_009C;
    }

    public static int lzf_decompress(byte[] input, ref byte[] output)
    {
        int length = input.Length;
        int num2 = output.Length;
        uint num3 = 0;
        uint num4 = 0;
        while (true)
        {
            uint num5 = input[num3++];
            if (num5 >= 0x20)
            {
                uint num6 = num5 >> 5;
                int num7 = ((int) (num4 - ((num5 & 0x1f) << 8))) - 1;
                if (num6 == 7)
                {
                    num6 += input[num3++];
                }
                num7 -= input[num3++];
                if (((num4 + num6) + 2) > num2)
                {
                    return 0;
                }
                if (num7 < 0)
                {
                    return 0;
                }
                output[num4++] = output[num7++];
                output[num4++] = output[num7++];
                do
                {
                    output[num4++] = output[num7++];
                }
                while (--num6 != 0);
            }
            else
            {
                num5++;
                if ((num4 + num5) > num2)
                {
                    return 0;
                }
                do
                {
                    output[num4++] = input[num3++];
                }
                while (--num5 != 0);
            }
            if (num3 >= length)
            {
                return (int) num4;
            }
        }
    }*/
}