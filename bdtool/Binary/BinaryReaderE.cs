using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bdtool.Models;
using static bdtool.Utilities.Binary;

namespace bdtool.Binary
{
    public sealed class BinaryReaderE
    {
        private readonly BinaryReader _reader;
        private readonly Endianness _endian;

        public BinaryReaderE(Stream input, Endianness endian)
        {
            _reader = new BinaryReader(input);
            _endian = endian;
        }

        public long Position => _reader.BaseStream.Position;
        public Stream BaseStream => _reader.BaseStream;

        public void Seek(long offset, SeekOrigin origin)
        {
            BaseStream.Seek(offset, origin);
        }

        public bool ReadBool()
        {
            if (BaseStream.Length - BaseStream.Position < 4)
                throw new EndOfStreamException();

            var bytes = _reader.ReadBytes(4);
            if (_endian == Endianness.Big)
                Array.Reverse(bytes);

            return BitConverter.ToInt32(bytes, 0) != 0;
        }

        public int ReadInt32()
        {
            if (BaseStream.Length - BaseStream.Position < 4)
                throw new EndOfStreamException();

            var bytes = _reader.ReadBytes(4);
            if (_endian == Endianness.Big)
                Array.Reverse(bytes);

            return BitConverter.ToInt32(bytes, 0);
        }

        public uint ReadUInt32()
        {
            if (BaseStream.Length - BaseStream.Position < 4)
                throw new EndOfStreamException();

            var bytes = _reader.ReadBytes(4);
            if (_endian == Endianness.Big)
                Array.Reverse(bytes);

            return BitConverter.ToUInt32(bytes, 0);
        }

        public ulong ReadUlong()
        {
            if (BaseStream.Length - BaseStream.Position < 8)
                throw new EndOfStreamException();

            var bytes = _reader.ReadBytes(8);
            if (_endian == Endianness.Big)
                Array.Reverse(bytes);

            return BitConverter.ToUInt64(bytes, 0);
        }

        public byte ReadUint8()
        {
            if (BaseStream.Length - BaseStream.Position < 1)
                throw new EndOfStreamException();

            var bytes = _reader.ReadBytes(1);
            return bytes[0];
        }

        public sbyte ReadInt8()
        {
            if (BaseStream.Length - BaseStream.Position < 1)
                throw new EndOfStreamException();

            var bytes = _reader.ReadBytes(1);
            
            return unchecked((sbyte)bytes[0]);
        }

        public float ReadFloat()
        {
            if (BaseStream.Length - BaseStream.Position < 4)
                throw new EndOfStreamException();

            var bytes = _reader.ReadBytes(4);
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
