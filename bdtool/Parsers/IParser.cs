using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bdtool.Binary;

namespace bdtool.Parsers
{
    public interface IParser<T>
    {
        T Read(BinaryReaderE br);
        void Write(BinaryWriterE bw, T obj);
    }
}
