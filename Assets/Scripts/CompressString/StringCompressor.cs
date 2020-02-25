namespace CompressString
{
    using System;
    using System.IO;
    using System.IO.Compression;
    using System.Text;

    internal static class StringCompressor
    {
        public static string CompressString(string text)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(text);
            MemoryStream compressedStream = new MemoryStream();
            using (GZipStream stream2 = new GZipStream(compressedStream, CompressionMode.Compress, true))
            {
                stream2.Write(bytes, 0, bytes.Length);
            }
            compressedStream.Position = 0L;
            byte[] buffer = new byte[compressedStream.Length];
            compressedStream.Read(buffer, 0, buffer.Length);
            byte[] dst = new byte[buffer.Length + 4];
            Buffer.BlockCopy(buffer, 0, dst, 4, buffer.Length);
            Buffer.BlockCopy(BitConverter.GetBytes(bytes.Length), 0, dst, 0, 4);
            return Convert.ToBase64String(dst);
        }

        public static string DecompressString(string compressedText)
        {
            byte[] buffer = Convert.FromBase64String(compressedText);
            using (MemoryStream stream = new MemoryStream())
            {
                int num = BitConverter.ToInt32(buffer, 0);
                stream.Write(buffer, 4, buffer.Length - 4);
                byte[] dest = new byte[num];
                stream.Position = 0L;
                using (GZipStream stream2 = new GZipStream(stream, CompressionMode.Decompress))
                {
                    stream2.Read(dest, 0, dest.Length);
                }
                return Encoding.UTF8.GetString(dest);
            }
        }
    }
}

