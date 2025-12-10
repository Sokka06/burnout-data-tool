using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bdtool.Utilities;

namespace bdtool.Models.B3
{
    /// <summary>
    /// 
    /// </summary>
    public record B3VehicleList() : Common.VList
    {
        public int VehicleCount { get; init; }
        public List<bool> VehicleIsDriveable { get; init; } = [];
        public List<int> RaceCarRanks { get; init; } = [];
        public List<ulong> VehicleIDs { get; init; } = [];
        public List<int> Unk1 { get; init; } = []; // TODO: figure out what's this
        public List<int> Unk2 { get; init; } = []; // TODO: figure out what's this
        public List<byte> Pad { get; init; } = []; // TODO: figure out if this really is just padding.

        public B3VehicleList(
        int vehicleCount,
        List<bool> vehicleIsDriveable,
        List<int> raceCarRanks,
        List<ulong> vehicleIDs,
        List<int> unk1,
        List<int> unk2,
        List<byte> pad,
        int versionNumber)
        : this()
        {
            VersionNumber = versionNumber;
            VehicleCount = vehicleCount;
            VehicleIsDriveable = vehicleIsDriveable;
            RaceCarRanks = raceCarRanks;
            VehicleIDs = vehicleIDs;
            Unk1 = unk1;
            Unk2 = unk2;
            Pad = pad;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.AppendLine($"Version Number: {VersionNumber}");
            builder.AppendLine($"Vehicle Count: {VehicleCount}");
            builder.AppendLine(string.Format("{0,-20} {1,-9} {2,-4} {3,-8} {4,-11} {5,-10}", "ID", "Name", "Rank", "Drivable", "Unk1", "Unk2"));
            for (int i = 0; i < VehicleCount; i++)
            {
                builder.AppendLine(string.Format("{0,-20} {1,-9} {2,-4} {3,-8} {4,-11} {5,-10}", VehicleIDs[i], GtID.GtIDUnCompress(VehicleIDs[i]).TrimEnd(), RaceCarRanks[i], VehicleIsDriveable[i], Unk1[i], Unk2[i]));
            }

            return builder.ToString();
        }
    }
}
