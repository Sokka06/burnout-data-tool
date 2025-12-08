using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bdtool.Utilities;

namespace bdtool.Models.B3
{
    public record B3VehicleList
    (
        int VersionNumber, // version 6 for BO3
        int VehicleCount,
        List<bool> VehicleIsDriveable, // 128
        List<int> RaceCarRanks, // 128
        List<ulong> VehicleIDs, // 128
        List<int> unk1, // 128
        List<int> unk2, // 128
        List<byte> Pad // 1016
    )
    {
        public B3VehicleList() : this(default, default, default, default, default, default, default, default)
        {
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.AppendLine($"Version Number: {VersionNumber}");
            builder.AppendLine($"Vehicle Count: {VehicleCount}");
            builder.AppendLine(string.Format("{0,-20} {1,-9} {2,-4} {3,-8} {4,-11} {5,-10}", "ID", "Name", "Rank", "Drivable", "Unk1", "Unk2"));
            for (int i = 0; i < VehicleCount; i++)
            {
                builder.AppendLine(string.Format("{0,-20} {1,-9} {2,-4} {3,-8} {4,-11} {5,-10}", VehicleIDs[i], GtID.GtIDUnCompress(VehicleIDs[i]).TrimEnd(), RaceCarRanks[i], VehicleIsDriveable[i], unk1[i], unk2[i]));
            }

            return builder.ToString();
        }
    }
}
