using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bdtool.Dto
{
    /// <summary>
    /// DTO for VList.
    /// </summary>
    public abstract record VehicleList
    {
        public int VersionNumber { get; init; }

        public VehicleList() { }
    }
}
