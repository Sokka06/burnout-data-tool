using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bdtool.Models.Common;
using bdtool.Utilities;

namespace bdtool.Parsers
{
    public class DatabaseFileDefParser : IParser<DatabaseFileDef>
    {
        public DatabaseFileDef Parse(EndianBinaryReader br)
        {
            var isActive = br.ReadBool();
            var fileHash = br.ReadInt32();
            return new Models.Common.DatabaseFileDef(isActive, fileHash);
        }
    }
}
