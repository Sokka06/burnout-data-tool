using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bdtool.Models.VDB
{
    public record DatabaseFileDef
    (
        bool IsActive,
        int FileHash
    )
    {
        public DatabaseFileDef() : this(default, default)
        {
        }

        public override string ToString()
        {
            return $"{FileHash}: IsActive: {IsActive}";
        }
    }
}
