using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bdtool.Dto.B3
{
    public record B3VehicleList : VehicleList
    {
        public required List<Entry> Entries { get; init; }

        public record Entry
        {
            public required string Id { get; init; }
            public bool Driveable { get; init; }
            public int Rank { get; init; }
            public int Unk1 { get; init; }
            public int Unk2 { get; init; }
        }
    }
}
