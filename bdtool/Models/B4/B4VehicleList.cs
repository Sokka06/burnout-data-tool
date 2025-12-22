using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bdtool.Utilities;

namespace bdtool.Models.B4
{
    /// <summary>
    /// TODO: confirm structure of VehicleList and the offsets.
    /// Length exactly 4096 bytes
    /// </summary>
    public record B4VehicleList() : Common.VList
    {
        public int VehicleCount { get; init; }
        public List<bool> VehicleIsDriveable { get; init; } = []; // 128
        public List<int> RaceCarRanks { get; init; } = []; // 128
        public List<ulong> VehicleIDs { get; init; } = []; // 128
        public List<int> VehicleMaxCrashScore { get; init; } = []; // 128
        public List<int> VehicleGrudgePoints { get; init; } = []; // 128
        public List<int> VehiclePrice { get; init; } = []; // 128
        public List<sbyte> VehicleDefaultColor { get; init; } = []; // 128
        public List<byte> Pad { get; init; } = []; // 376

        public B4VehicleList(
        int vehicleCount,
        List<bool> vehicleIsDriveable,
        List<int> raceCarRanks,
        List<ulong> vehicleIDs,
        List<int> vehicleMaxCrashScore,
        List<int> vehicleGrudgePoints,
        List<int> vehiclePrice,
        List<sbyte> vehicleDefaultColor,
        List<byte> pad,
        int versionNumber)
        : this()
        {
            VersionNumber = versionNumber;
            VehicleCount = vehicleCount;
            VehicleIsDriveable = vehicleIsDriveable;
            RaceCarRanks = raceCarRanks;
            VehicleIDs = vehicleIDs;
            VehicleMaxCrashScore = vehicleMaxCrashScore;
            VehicleGrudgePoints = vehicleGrudgePoints;
            VehiclePrice = vehiclePrice;
            VehicleDefaultColor = vehicleDefaultColor;
            Pad = pad;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.AppendLine($"Version Number: {VersionNumber}");
            builder.AppendLine($"Vehicle Count: {VehicleCount}");
            //builder.AppendLine($"ID     Rank    IsDrivable  MaxCrashScore   GrudgePoints    Price   DefaultColor");
            builder.AppendLine(string.Format("{0,-20} {1,-12} {2,-4} {3,-8} {4,-13} {5,-12} {6,-6} {7,-10}", "GtId", "ID", "Rank", "Drivable", "MaxCrashScore", "GrudgePoints", "Price", "DefaultColor"));
            for (int i = 0; i < VehicleCount; i++)
            {
                builder.AppendLine(string.Format("{0,-20} {1,-12} {2,-4} {3,-8} {4,-13} {5,-12} {6,-6} {7,-10}", VehicleIDs[i], GtID.Uncompress(VehicleIDs[i]).TrimEnd(), RaceCarRanks[i], VehicleIsDriveable[i], VehicleMaxCrashScore[i], VehicleGrudgePoints[i], VehiclePrice[i], VehicleDefaultColor[i]));
                //builder.AppendLine($"{VehicleIDs[i]} ({GtID.GtIDUnCompress(VehicleIDs[i]).TrimEnd()})    {RaceCarRanks[i]}   {VehicleIsDriveable[i]} {VehicleMaxCrashScore[i]}   {VehicleGrudgePoints[i]}   {VehiclePrice[i]}  {VehicleDefaultColor[i]}");
            }

            return builder.ToString();
        }
    }
}
