using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bdtool.Binary;
using bdtool.Models.B4;
using bdtool.Models.Common;

namespace bdtool.Parsers
{
    public class B4VehicleListParser : IParser<B4VehicleList>
    {

        public B4VehicleList Read(EndianBinaryReader br)
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

            var vehicleMaxCrashScore = new List<int>();
            for (int i = 0; i < 128; i++)
            {
                vehicleMaxCrashScore.Add(br.ReadInt32());
            }

            var vehicleGrudgePoints = new List<int>();
            for (int i = 0; i < 128; i++)
            {
                vehicleGrudgePoints.Add(br.ReadInt32());
            }

            var vehiclePrice = new List<int>();
            for (int i = 0; i < 128; i++)
            {
                vehiclePrice.Add(br.ReadInt32());
            }

            var vehicleDefaultColor = new List<sbyte>();
            for (int i = 0; i < 128; i++)
            {
                vehicleDefaultColor.Add(br.ReadInt8());
            }

            var pad = new List<byte>();
            for (int i = 0; i < 376; i++)
            {
                pad.Add(br.ReadUint8());
            }

            return new Models.B4.B4VehicleList
            (
                versionNumber, 
                vehicleCount, 
                vehicleIsDriveable, 
                raceCarRanks, 
                vehicleIDs, 
                vehicleMaxCrashScore, 
                vehicleGrudgePoints, 
                vehiclePrice, 
                vehicleDefaultColor, 
                pad
            );
        }

        public void Write(EndianBinaryWriter bw, B4VehicleList obj)
        {

        }
    }
}