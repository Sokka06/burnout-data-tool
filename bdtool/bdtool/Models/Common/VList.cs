using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bdtool.Models.Common
{
    public record VList
    {
        public int VersionNumber { get; init; }

        public VList() { }
    }
}
