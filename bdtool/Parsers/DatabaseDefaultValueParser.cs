using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bdtool.Binary;
using bdtool.Models.Common;

namespace bdtool.Parsers
{

    public class DatabaseDefaultValueParser : IParser<DatabaseDefaultValue>
    {

        public DatabaseDefaultValue Read(BinaryReaderE br)
        {
            var data = new Models.Common.DataElement(br.ReadInt32());
            var nameHash = br.ReadInt32();
            return new Models.Common.DatabaseDefaultValue(nameHash, data);

            //var nameBytes = br.ReadBytes(32);
            //string version = Encoding.UTF8.GetString(nameBytes).TrimEnd('\0');
            //return new Models.FileHeader(version);
        }

        public void Write(BinaryWriterE bw, DatabaseDefaultValue obj)
        {
            bw.WriteInt32(obj.Data.RawValue);
            bw.WriteInt32(obj.NameHash);
        }
    }
}
