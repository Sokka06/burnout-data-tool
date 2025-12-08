using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bdtool.Models;
using static bdtool.Utilities.Binary;

namespace bdtool.Binary
{
    public sealed class EndianBinaryReader
    {
        private readonly BinaryReader _br;
        private readonly Endianness _endian;

        public EndianBinaryReader(Stream input, Endianness endian)
        {
            _br = new BinaryReader(input);
            _endian = endian;
        }

        public long Position => _br.BaseStream.Position;

        public void Seek(long offset, SeekOrigin origin)
        {
            _br.BaseStream.Seek(offset, origin);
        }

        public bool ReadBool()
        {
            var bytes = _br.ReadBytes(4);
            if (_endian == Endianness.Big)
                Array.Reverse(bytes);

            return BitConverter.ToInt32(bytes, 0) != 0;
        }

        public int ReadInt32()
        {
            var bytes = _br.ReadBytes(4);
            if (_endian == Endianness.Big)
                Array.Reverse(bytes);

            return BitConverter.ToInt32(bytes, 0);
        }

        public uint ReadUInt32()
        {
            var bytes = _br.ReadBytes(4);
            if (_endian == Endianness.Big)
                Array.Reverse(bytes);

            return BitConverter.ToUInt32(bytes, 0);
        }

        public ulong ReadUlong()
        {
            var bytes = _br.ReadBytes(8);
            if (_endian == Endianness.Big)
                Array.Reverse(bytes);

            return BitConverter.ToUInt64(bytes, 0);
        }

        public byte ReadUint8()
        {
            var bytes = _br.ReadBytes(1);
            return bytes[0];
        }

        public sbyte ReadInt8()
        {
            var bytes = _br.ReadBytes(1);
            
            return unchecked((sbyte)bytes[0]);
        }

        public float ReadFloat()
        {
            var bytes = _br.ReadBytes(4);
            if (_endian == Endianness.Big)
                Array.Reverse(bytes);

            return BitConverter.ToSingle(bytes, 0);
        }

        /*public DataElement ReadDataElement()
        {
            var bytes = _br.ReadBytes(4);
            if (_endian == Endianness.Big)
                Array.Reverse(bytes);

            return new DataElement(BitConverter.ToInt32(bytes, 0));
        }*/

    }
}
