using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoProductionChains.Accessories.Util
{
    public static class BlueprintHandler
    {
        public static string EncodeBP(string jsonData)
        {
            // 
            byte[] compressedBytes = ZlibDeflate(jsonData);
            return "0" + Convert.ToBase64String(compressedBytes);
        }

        public static string DecodeBP(string bpString)
        {
            // Remove version byte
            bpString = bpString.Remove(0, 1);

            // Base64 string lengths must be a multiple of 4
            int mod4 = bpString.Length % 4;
            if (mod4 > 0)
            {
                bpString += new string('=', 4 - mod4);
            }
            // Convert from base64
            byte[] bpBytes = Convert.FromBase64String(bpString);

            return ZlibInflate(bpBytes);
        }

        private static string ZlibInflate(byte[] bytes)
        {
            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var ds = new GZipStream(msi, CompressionMode.Decompress))
                {
                    ds.CopyTo(mso);
                }

                return Encoding.UTF8.GetString(mso.ToArray());
            }
        }

        private static byte[] ZlibDeflate(string str)
        {
            CompressionLevel compression = CompressionLevel.Optimal;
            var inputBytes = Encoding.UTF8.GetBytes(str);

            using (var msi = new MemoryStream(inputBytes))
            using (var mso = new MemoryStream())
            {
                using (var ds = new DeflateStream(mso, compression))
                {
                    msi.Position = 0;
                    msi.CopyTo(ds);
                }

                var compressedBytes = mso.ToArray();

                // Add our ZLIB Header bytes
                compressedBytes = AddZlibHeader(compressedBytes, compression);


                // Add adler32 checksum
                byte[] checksum = GetAdler32Checksum(inputBytes, 0, inputBytes.Length);
                // Add it to our previous data
                byte[] finalBytes = new byte[compressedBytes.Length + checksum.Length];
                compressedBytes.CopyTo(finalBytes, 0);
                checksum.CopyTo(finalBytes, compressedBytes.Length);

                return finalBytes;
            }
        }

        private enum Adler32
        {
            Modulus = 65521
        }
        private static byte[] GetAdler32Checksum(byte[] data, int offset, int length)
        {
            // Adler32 algorithm
            int a = 1;
            int b = 0;
            for (int counter = 0; counter < length; ++counter)
            {
                a = (a + (data[offset + counter])) % (int)Adler32.Modulus;
                b = (b + a) % (int)Adler32.Modulus;
            }
            int checksum = (b * 65536) + a;

            // Get bytes from our checksum
            byte[] intBytes = BitConverter.GetBytes(checksum);
            // BitConverter reverses the bytes, so we need to reverse them again
            Array.Reverse(intBytes);
            return intBytes;
        }


        // Adds Zlib header based on compression
        private static byte[] AddZlibHeader(byte[] bArray, CompressionLevel compression)
        {
            byte[] newArray = new byte[bArray.Length + 2];
            bArray.CopyTo(newArray, 2);
            newArray[0] = 0x78;

            if (compression == CompressionLevel.NoCompression)
                newArray[1] = 0x01;
            if (compression == CompressionLevel.Fastest)
                newArray[1] = 0x9C;
            if (compression == CompressionLevel.Optimal)
                newArray[1] = 0xDA;

            return newArray;
        }
    }
}
