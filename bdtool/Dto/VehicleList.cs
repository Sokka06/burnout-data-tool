using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;

namespace bdtool.Dto
{
    /// <summary>
    /// DTO for VList.
    /// </summary>
    public abstract record VehicleList
    {
        [YamlMember(Order = -1)]
        public int VersionNumber { get; init; }

        public VehicleList() { }
    }
}
