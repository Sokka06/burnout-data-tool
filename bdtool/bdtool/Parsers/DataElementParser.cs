using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bdtool.Models.Common;
using bdtool.Utilities;

namespace bdtool.Parsers
{

    public class DataElementParser : IParser<DataElement>
    {
        public DataElement Parse(EndianBinaryReader br)
        {
            throw new NotImplementedException();

            //var nameBytes = br.ReadBytes(32);
            //string version = Encoding.UTF8.GetString(nameBytes).TrimEnd('\0');
            //return new Models.FileHeader(version);
        }
    }
}
