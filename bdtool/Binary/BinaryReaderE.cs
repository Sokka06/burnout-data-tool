using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bdtool.Models;
using bdtool.Models.Types;
using static bdtool.Utilities.Binary;

namespace bdtool.Binary
{
    public sealed class BinaryReaderE
    {
        private readonly BinaryReader _reader;
        private readonly Endian _endian;

        public BinaryReaderE(Stream input, Endian endian)
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
            if (_endian == Endian.Big)
                Array.Reverse(bytes);

            return BitConverter.ToInt32(bytes, 0) != 0;
        }

        public int ReadInt32()
        {
            if (BaseStream.Length - BaseStream.Position < 4)
                throw new EndOfStreamException();

            var bytes = _reader.ReadBytes(4);
            if (_endian == Endian.Big)
                Array.Reverse(bytes);

            return BitConverter.ToInt32(bytes, 0);
        }

        public uint ReadUInt32()
        {
            if (BaseStream.Length - BaseStream.Position < 4)
                throw new EndOfStreamException();

            var bytes = _reader.ReadBytes(4);
            if (_endian == Endian.Big)
                Array.Reverse(bytes);

            return BitConverter.ToUInt32(bytes, 0);
        }

        public ulong ReadUlong()
        {
            if (BaseStream.Length - BaseStream.Position < 8)
                throw new EndOfStreamException();

            var bytes = _reader.ReadBytes(8);
            if (_endian == Endian.Big)
                Array.Reverse(bytes);

            return BitConverter.ToUInt64(bytes, 0);
        }

        public short ReadInt16()
        {
            if (BaseStream.Length - BaseStream.Position < 2)
                throw new EndOfStreamException();

            var bytes = _reader.ReadBytes(2);

            return BitConverter.ToInt16(bytes, 0);
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
            if (_endian == Endian.Big)
                Array.Reverse(bytes);

            return BitConverter.ToSingle(bytes, 0);
        }

        public V3d ReadV3d()
        {
            var values = new float[4];

            // read 3 floats and padding
            for (int i = 0; i < 4; i++)
            {
                if (BaseStream.Length - BaseStream.Position < 4)
                    throw new EndOfStreamException();

                var bytes = _reader.ReadBytes(4);
                if (_endian == Endian.Big)
                    Array.Reverse(bytes);

                values[i] = BitConverter.ToSingle(bytes, 0);
            }

            return new V3d() { X = values[0], Y = values[1], Z = values[2] };
        }

        public V3dPlus ReadV3dPlus()
        {
            var values = new float[4];

            // read 4 floats
            for (int i = 0; i < 4; i++)
            {
                if (BaseStream.Length - BaseStream.Position < 4)
                    throw new EndOfStreamException();

                var bytes = _reader.ReadBytes(4);
                if (_endian == Endian.Big)
                    Array.Reverse(bytes);

                values[i] = BitConverter.ToSingle(bytes, 0);
            }

            return new V3dPlus() { X = values[0], Y = values[1], Z = values[2], Plus = values[3] };
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
