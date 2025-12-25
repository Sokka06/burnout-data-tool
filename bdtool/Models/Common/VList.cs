using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;

namespace bdtool.Models.Common
{
    public record VList
    {
        [YamlMember(Order = -1)]
        public int VersionNumber { get; init; }

        public VList() { }
    }
}
