using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bdtool.Models.VDB
{
    public record DatabaseDefaultValue
    {
        public int NameHash { get; init; }
        public required DataElement Data { get; init; }

        public const int DEFAULT_VALUE_LENGTH = 8;

        /*public DatabaseDefaultValue() : this(default, default)
        {
        }*/

        public override string ToString()
        {
            return $"{NameHash}: {Data}";
        }
    }
}
