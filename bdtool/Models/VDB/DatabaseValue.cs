using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bdtool.Models.VDB
{
    public record DatabaseValue
    {
        public long Address { get; init; }
        public required DataElement Value { get; init; }

        //public int Value { get; init; }

        public const int VALUE_LENGTH = 4;

        /*public DatabaseValue() : this(default, default)
        {
        }*/

        public override string ToString()
        {
            return $"{Address}: {Value}";
        }
    }
}
