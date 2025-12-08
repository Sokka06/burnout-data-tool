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
        T Read(EndianBinaryReader br);
        void Write(EndianBinaryWriter bw, T obj);
    }
}
