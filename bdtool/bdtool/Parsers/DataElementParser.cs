using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bdtool.Binary;
using bdtool.Models.Common;

namespace bdtool.Parsers
{

    public class DataElementParser : IParser<DataElement>
    {

        public DataElement Read(EndianBinaryReader br)
        {
            return new DataElement(0);
        }

        public void Write(EndianBinaryWriter bw, DataElement obj)
        {

        }
    }
}
