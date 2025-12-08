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
        float Value0,
        float Value1,
        float Value2,
        float Value3
    )
    {
        public override string ToString()
        {
            return $"{Address}: ({Value0}, {Value1}, {Value2}, {Value3})";
        }
    }
}
