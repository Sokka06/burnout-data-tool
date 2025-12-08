using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bdtool.Models.Common
{
    public record DatabaseFileDef
    (
        bool IsActive,
        int FileHash
    )
    {
        public override string ToString()
        {
            return $"{FileHash}: IsActive: {IsActive}";
        }
    }
}
