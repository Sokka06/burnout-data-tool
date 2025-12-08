using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bdtool.Utilities;

namespace bdtool.Models.B4
{
    public record B4VehicleList
    (
        int VersionNumber, // version 9 for BO4
        int VehicleCount,
        List<bool> VehicleIsDriveable, // 128
        List<int> RaceCarRanks, // 128
        List<ulong> VehicleIDs, // 128
        List<int> VehicleMaxCrashScore, // 128
        List<int> VehicleGrudgePoints, // 128
        List<int> VehiclePrice, // 128
        List<sbyte> VehicleDefaultColor, // 128
        List<byte> Pad // 376
    )
    {
        public B4VehicleList() : this(default, default, default, default, default, default, default, default, default, default)
        {
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.AppendLine($"Version Number: {VersionNumber}");
            builder.AppendLine($"Vehicle Count: {VehicleCount}");
            //builder.AppendLine($"ID     Rank    IsDrivable  MaxCrashScore   GrudgePoints    Price   DefaultColor");
            builder.AppendLine(string.Format("{0,-20} {1,-12} {2,-4} {3,-8} {4,-13} {5,-12} {6,-6} {7,-10}", "ID", "Name", "Rank", "Drivable", "MaxCrashScore", "GrudgePoints", "Price", "DefaultColor"));
            for (int i = 0; i < VehicleCount; i++)
            {
                builder.AppendLine(string.Format("{0,-20} {1,-12} {2,-4} {3,-8} {4,-13} {5,-12} {6,-6} {7,-10}", VehicleIDs[i], GtID.GtIDUnCompress(VehicleIDs[i]).TrimEnd(), RaceCarRanks[i], VehicleIsDriveable[i], VehicleMaxCrashScore[i], VehicleGrudgePoints[i], VehiclePrice[i], VehicleDefaultColor[i]));
                //builder.AppendLine($"{VehicleIDs[i]} ({GtID.GtIDUnCompress(VehicleIDs[i]).TrimEnd()})    {RaceCarRanks[i]}   {VehicleIsDriveable[i]} {VehicleMaxCrashScore[i]}   {VehicleGrudgePoints[i]}   {VehiclePrice[i]}  {VehicleDefaultColor[i]}");
            }

            return builder.ToString();
        }
    }
}
