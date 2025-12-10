using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bdtool.Dto.B4
{
    public record B4VehicleList : VehicleList
    {
        public required List<Entry> Entries { get; init; }

        public record Entry
        {
            public required string Id { get; init; }
            public bool Driveable { get; init; }
            public int Rank { get; init; }
            public int MaxCrashScore { get; init; }
            public int GrudgePoints { get; init; }
            public int Price { get; init; }
            public sbyte DefaultColor { get; init; }
        }
    }
}
