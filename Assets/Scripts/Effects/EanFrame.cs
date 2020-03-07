using System;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential)]
public struct EanFrame
{
    public ushort X;
    public ushort Y;
    public ushort Width;
    public ushort Height;
}

