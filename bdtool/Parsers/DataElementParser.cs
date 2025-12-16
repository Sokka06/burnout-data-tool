using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bdtool.Binary;
using bdtool.Models.VDB;

namespace bdtool.Parsers
{

    public class DataElementParser : IParser<DataElement>
    {

        public DataElement Read(BinaryReaderE br)
        {
            return new DataElement(0);
        }

        public void Write(BinaryWriterE bw, DataElement obj)
        {

        }
    }
}
