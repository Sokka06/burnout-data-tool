using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using bdtool.Models;
using YamlDotNet.Core.Tokens;
using static bdtool.Utilities.Binary;

namespace bdtool.Binary
{
    public sealed class BinaryWriterE
    {
        private readonly BinaryWriter _writer;
        private readonly Endianness _endian;

        public BinaryWriterE(Stream output, Endianness endian)
        {
            _writer = new BinaryWriter(output);
            _endian = endian;
        }

        public long Position => _writer.BaseStream.Position;
        public Stream BaseStream => _writer.BaseStream;

        public void Seek(long offset, SeekOrigin origin)
        {
            BaseStream.Seek(offset, origin);
        }

        public void WriteBytes(byte[] bytes)
        {
            if (_endian == Endianness.Big)
                Array.Reverse(bytes);

            _writer.Write(bytes);
        }

        public void WriteInt32(int value)
        {
            var bytes = BitConverter.GetBytes(value);
            if (_endian == Endianness.Big)
                Array.Reverse(bytes);

            _writer.Write(bytes);
        }

        public void WriteBool(bool value)
        {
            var asInt32 = Convert.ToInt32(value);
            var bytes = BitConverter.GetBytes(asInt32);
            if (_endian == Endianness.Big)
                Array.Reverse(bytes);

            _writer.Write(bytes);
        }

        public void WriteUlong(ulong value)
        {
            var bytes = BitConverter.GetBytes(value);
            if (_endian == Endianness.Big)
                Array.Reverse(bytes);

            _writer.Write(bytes);
        }

        public void WriteUint8(byte value)
        {
            _writer.Write(value);
        }

        public void WriteInt8(sbyte value)
        {
            _writer.Write(unchecked((byte)value));
        }
    }
}
