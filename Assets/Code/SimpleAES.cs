using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public class SimpleAES
{
    private ICryptoTransform decryptor;
    private UTF8Encoding encoder;
    private ICryptoTransform encryptor;
    private static byte[] key = new byte[] { 
        0x7b, 0xd9, 0x13, 11, 0x18, 0x1a, 0x55, 0x2d, 0x72, 0xb8, 0x1b, 0xa2, 0x25, 0x70, 0xde, 0xd1, 
        0xf1, 0x18, 0xaf, 0x90, 0xad, 0x35, 0xc4, 0x1d, 0x18, 0x1a, 0x11, 0xda, 0x83, 0xec, 0x35, 0xd1
     };
    private static byte[] vector = new byte[] { 0x92, 0x40, 0xbf, 0x6f, 0x17, 3, 0x71, 0x77, 0xe7, 0x79, 0xdd, 0x70, 0x4f, 0x20, 0x72, 0x9c };

    public SimpleAES()
    {
        RijndaelManaged managed = new RijndaelManaged();
        this.encryptor = managed.CreateEncryptor(key, vector);
        this.decryptor = managed.CreateDecryptor(key, vector);
        this.encoder = new UTF8Encoding();
    }

    public string Decrypt(string encrypted)
    {
        return this.encoder.GetString(this.Decrypt(Convert.FromBase64String(encrypted)));
    }

    public byte[] Decrypt(byte[] buffer)
    {
        return this.Transform(buffer, this.decryptor);
    }

    public string Encrypt(string unencrypted)
    {
        return Convert.ToBase64String(this.Encrypt(this.encoder.GetBytes(unencrypted)));
    }

    public byte[] Encrypt(byte[] buffer)
    {
        return this.Transform(buffer, this.encryptor);
    }

    protected byte[] Transform(byte[] buffer, ICryptoTransform transform)
    {
        MemoryStream stream = new MemoryStream();
        using (CryptoStream stream2 = new CryptoStream(stream, transform, CryptoStreamMode.Write))
        {
            stream2.Write(buffer, 0, buffer.Length);
        }
        return stream.ToArray();
    }
}

