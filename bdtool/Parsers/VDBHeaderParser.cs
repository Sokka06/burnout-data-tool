using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bdtool.Binary;
using bdtool.Models.VDB;

namespace bdtool.Parsers
{
    public class VDBHeaderParser : IParser<VDBHeader>
    {
        public VDBHeader Read(BinaryReaderE br)
        {
            var type = br.ReadInt32();
            var defaultValueCount = br.ReadInt32();
            var unk1 = br.ReadInt32();
            var fileDefCount = br.ReadInt32();
            var fileDefOffset = br.ReadInt32();

            return new Models.VDB.VDBHeader(type, defaultValueCount, unk1, fileDefCount, fileDefOffset);
        }

        public void Write(BinaryWriterE bw, VDBHeader obj)
        {
            bw.WriteInt32(obj.Type);
            bw.WriteInt32(obj.DefaultValueCount);
            bw.WriteInt32(obj.Unk1);
            bw.WriteInt32(obj.FileDefCount);
            bw.WriteInt32(obj.FileDefOffset);
        }
    }
}
