using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bdtool.Utilities;

namespace bdtool.Parsers
{
    public interface IParser<T>
    {
        T Parse(EndianBinaryReader br);
    }
}
