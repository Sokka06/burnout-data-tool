using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bdtool.Models.Common;
using bdtool.Utilities;

namespace bdtool.Parsers
{
    public class DatabaseValueParser : IParser<DatabaseValue>
    {
        public DatabaseValue Parse(EndianBinaryReader br)
        {
            var address = br.Position;
            var value0 = br.ReadFloat();
            var value1 = br.ReadFloat();
            var value2 = br.ReadFloat();
            var value3 = br.ReadFloat();
            return new Models.Common.DatabaseValue(address, value0, value1, value2, value3);
        }
    }
}
