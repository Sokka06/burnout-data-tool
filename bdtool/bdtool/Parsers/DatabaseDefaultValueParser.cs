using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bdtool.Models.Common;
using bdtool.Utilities;

namespace bdtool.Parsers
{

    public class DatabaseDefaultValueParser : IParser<DatabaseDefaultValue>
    {
        public DatabaseDefaultValue Parse(EndianBinaryReader br)
        {
            var data = new Models.Common.DataElement(br.ReadInt32());
            var nameHash = br.ReadInt32();
            return new Models.Common.DatabaseDefaultValue(data, nameHash);

            //var nameBytes = br.ReadBytes(32);
            //string version = Encoding.UTF8.GetString(nameBytes).TrimEnd('\0');
            //return new Models.FileHeader(version);
        }
    }
}
