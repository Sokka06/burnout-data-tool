using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bdtool.Utilities
{
    public static class Vehicle
    {
        /// <summary>
        /// Creates a path from vehicle ID.
        /// 10358822303267206920 (LOWRUSCAR1A) -> pveh/LOWR/US/CAR1A
        /// 
        /// CreateVehiclePathFromID__14CB4VehicleListUlPci from BO4 PS2 Prototype.
        /// </summary>
        public static string CreateVehiclePathFromID(ulong vehicleId)
        {
            var id = GtID.GtIDUnCompress(vehicleId).TrimEnd(' ', '\0');

            // Split into segments: first 4, next 2, rest
            string part1 = id.Length > 0 ? id.Substring(0, Math.Min(4, id.Length)) : "";
            string part2 = id.Length > 4 ? id.Substring(4, Math.Min(2, id.Length - 4)) : "";
            string part3 = id.Length > 6 ? id.Substring(6) : "";

            return $"pveh/{part1}/{part2}/{part3}";
        }

        /// <summary>
        /// Second version
        /// </summary>
        /// <param name="vehicleId"></param>
        /// <returns></returns>
        public static string CreateVehiclePathFromID2(ulong vehicleId)
        {
            var id = GtID.GtIDUnCompress(vehicleId);

            // Replicate the C trimming logic: find first space and truncate
            int nullPos = id.IndexOf(' ');
            if (nullPos >= 0)
                id = id.Substring(0, nullPos);

            // Extract segments (strncat behavior: copy up to N chars or until null)
            string part1 = id.Length > 0 ? id.Substring(0, Math.Min(4, id.Length)) : "";
            string part2 = id.Length > 4 ? id.Substring(4, Math.Min(2, id.Length - 4)) : "";
            string part3 = id.Length > 6 ? id.Substring(6) : "";

            return $"pveh/{part1}/{part2}/{part3}";
        }
    }
}
