using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bdtool.Binary;
using bdtool.Models.Common;

namespace bdtool.Parsers
{
    public class DatabaseFileDefParser : IParser<DatabaseFileDef>
    {

        public DatabaseFileDef Read(BinaryReaderE br)
        {
            var isActive = br.ReadBool();
            var fileHash = br.ReadInt32();
            return new Models.Common.DatabaseFileDef(isActive, fileHash);
        }

        public void Write(BinaryWriterE bw, DatabaseFileDef obj)
        {
            bw.WriteBool(obj.IsActive);
            bw.WriteInt32(obj.FileHash);
        }
    }
}
