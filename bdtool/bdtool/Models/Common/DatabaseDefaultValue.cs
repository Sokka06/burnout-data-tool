using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bdtool.Models.Common
{
    public record DatabaseDefaultValue
    (
        DataElement Data,
        int NameHash
    )
    {
        public override string ToString()
        {
            return $"{NameHash}: {Data}";
        }
    }
}
