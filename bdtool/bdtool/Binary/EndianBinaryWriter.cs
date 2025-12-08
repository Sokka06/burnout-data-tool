using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bdtool.Models;
using static bdtool.Utilities.Binary;

namespace bdtool.Binary
{
    public sealed class EndianBinaryWriter
    {
        private readonly BinaryWriter _br;
        private readonly Endianness _endian;

        public EndianBinaryWriter(Stream output, Endianness endian)
        {
            _br = new BinaryWriter(output);
            _endian = endian;
        }

        public long Position => _br.BaseStream.Position;

        public void Seek(long offset, SeekOrigin origin)
        {
            _br.BaseStream.Seek(offset, origin);
        }

        public void WriteBytes(byte[] bytes)
        {
            if (_endian == Endianness.Big)
                Array.Reverse(bytes);

            _br.Write(bytes);
        }

        public void WriteInt32(int value)
        {
            var bytes = BitConverter.GetBytes(value);
            if (_endian == Endianness.Big)
                Array.Reverse(bytes);
            _br.Write(bytes);
        }

        public void WriteBool(bool value)
        {
            var asInt32 = Convert.ToInt32(value);
            var bytes = BitConverter.GetBytes(asInt32);
            if (_endian == Endianness.Big)
                Array.Reverse(bytes);
            _br.Write(bytes);
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
