using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bdtool.Models.Common
{
    public record DatabaseDefaultValue
    (
        int NameHash,
        DataElement Data
    )
    {
        public DatabaseDefaultValue() : this(default, default)
        {
        }

        public override string ToString()
        {
            return $"{NameHash}: {Data}";
        }
    }
}
