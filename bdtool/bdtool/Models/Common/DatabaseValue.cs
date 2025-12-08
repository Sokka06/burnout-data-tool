using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bdtool.Models.Common
{
    public record DatabaseValue
    (
        long Address, // not stored in the file.
        int Value
    )
    {

        public DatabaseValue() : this(default, default)
        {
        }

        public override string ToString()
        {
            return $"{Address}: {Value}";
        }
    }
}
