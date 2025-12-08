using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bdtool.Models.B3;
using bdtool.Utilities;

namespace bdtool.Parsers
{
    public class B3VehicleListParser : IParser<B3VehicleList>
    {
        public B3VehicleList Parse(EndianBinaryReader br)
        {
            var versionNumber = br.ReadInt32();
            var vehicleCount = br.ReadInt32();

            var vehicleIsDriveable = new List<bool>();
            for (int i = 0; i < 128; i++)
            {
                vehicleIsDriveable.Add(br.ReadBool());
            }

            var raceCarRanks = new List<int>();
            for (int i = 0; i < 128; i++)
            {
                raceCarRanks.Add(br.ReadInt32());
            }

            var vehicleIDs = new List<ulong>();
            for (int i = 0; i < 128; i++)
            {
                vehicleIDs.Add(br.ReadUlong());
            }

            var unk1 = new List<int>();
            for (int i = 0; i < 128; i++)
            {
                unk1.Add(br.ReadInt32());
            }

            var unk2 = new List<int>();
            for (int i = 0; i < 128; i++)
            {
                unk2.Add(br.ReadInt32());
            }

            var pad = new List<byte>();
            for (int i = 0; i < 1016; i++)
            {
                pad.Add(br.ReadUint8());
            }

            return new Models.B3.B3VehicleList(versionNumber, vehicleCount, vehicleIsDriveable, raceCarRanks, vehicleIDs, unk1, unk2, pad);
        }
    }
}
