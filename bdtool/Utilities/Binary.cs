using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bdtool.Utilities
{
    public static class  Binary
    {
        public enum Endian
        {
            Little,
            Big
        }

        public static Endian DetectEndian(byte[] headerBytes)
        {
            var le = BitConverter.ToInt32(headerBytes, 0);
            var be = BitConverter.ToInt32(headerBytes.Reverse().ToArray(), 0);

            if (le is >= 0 and <= 1000) return Endian.Little;
            if (be is >= 0 and <= 1000) return Endian.Big;

            throw new InvalidDataException("Unable to detect byte order.");
        }

        public static byte[] Reverse(ref byte[] bytes)
        {
            Array.Reverse(bytes);
            return bytes;
        }

        public static byte[] HexToBytes(string hex)
        {
            if (hex.Length % 2 != 0)
                throw new ArgumentException("Hex string must have even length.");

            var bytes = new byte[hex.Length / 2];
            for (int i = 0; i < bytes.Length; i++)
                bytes[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);

            return bytes;
        }

        public static string BytesToHex(byte[] bytes)
        {
            var sb = new StringBuilder(bytes.Length * 2);
            foreach (byte b in bytes)
                sb.Append($"{b:X2}");
            return sb.ToString();
        }
    }
}
