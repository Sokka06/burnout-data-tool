using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bdtool.Models.Common;
using bdtool.Utilities;

namespace bdtool.Parsers
{
    public class VDBHeaderParser : IParser<VDBHeader>
    {
        public VDBHeader Parse(EndianBinaryReader br)
        {
            var type = br.ReadInt32();
            var defaultValueCount = br.ReadInt32();
            var unk1 = br.ReadInt32();
            var fileDefCount = br.ReadInt32();
            var fileDefOffset = br.ReadInt32();

            return new Models.Common.VDBHeader(type, defaultValueCount, unk1, fileDefCount, fileDefOffset);
        }
    }
}
