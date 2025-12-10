using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bdtool.Binary;
using bdtool.Models.B4;
using bdtool.Models.Common;

namespace bdtool.Parsers.VList
{
    public class B4VehicleListParser : VListParser
    {
        private const int MAX_VEHICLES = 128;
        private const long MAX_LENGTH = 4096;

        public override B4VehicleList Read(BinaryReaderE br)
        {
            // TODO: add console logs
            var versionNumber = br.ReadInt32();
            var vehicleCount = br.ReadInt32();

            var vehicleIsDriveable = new List<bool>();
            for (int i = 0; i < vehicleCount; i++)
            {
                vehicleIsDriveable.Add(br.ReadBool());
            }

            // seek forward if necessary
            if (vehicleCount < MAX_VEHICLES)
                br.Seek((MAX_VEHICLES - vehicleCount) * 4, SeekOrigin.Current);

            var raceCarRanks = new List<int>();
            for (int i = 0; i < vehicleCount; i++)
            {
                raceCarRanks.Add(br.ReadInt32());
            }

            // seek forward if necessary
            if (vehicleCount < MAX_VEHICLES)
                br.Seek((MAX_VEHICLES - vehicleCount) * 4, SeekOrigin.Current);

            var vehicleIDs = new List<ulong>();
            for (int i = 0; i < vehicleCount; i++)
            {
                vehicleIDs.Add(br.ReadUlong());
            }

            // seek forward if necessary
            if (vehicleCount < MAX_VEHICLES)
                br.Seek((MAX_VEHICLES - vehicleCount) * 8, SeekOrigin.Current);

            var vehicleMaxCrashScore = new List<int>();
            for (int i = 0; i < vehicleCount; i++)
            {
                vehicleMaxCrashScore.Add(br.ReadInt32());
            }

            // seek forward if necessary
            if (vehicleCount < MAX_VEHICLES)
                br.Seek((MAX_VEHICLES - vehicleCount) * 4, SeekOrigin.Current);

            var vehicleGrudgePoints = new List<int>();
            for (int i = 0; i < vehicleCount; i++)
            {
                vehicleGrudgePoints.Add(br.ReadInt32());
            }

            // seek forward if necessary
            if (vehicleCount < MAX_VEHICLES)
                br.Seek((MAX_VEHICLES - vehicleCount) * 4, SeekOrigin.Current);

            var vehiclePrice = new List<int>();
            for (int i = 0; i < vehicleCount; i++)
            {
                vehiclePrice.Add(br.ReadInt32());
            }

            // seek forward if necessary
            if (vehicleCount < MAX_VEHICLES)
                br.Seek((MAX_VEHICLES - vehicleCount) * 4, SeekOrigin.Current);

            var vehicleDefaultColor = new List<sbyte>();
            for (int i = 0; i < vehicleCount; i++)
            {
                vehicleDefaultColor.Add(br.ReadInt8());
            }

            // seek forward if necessary
            if (vehicleCount < MAX_VEHICLES)
                br.Seek((MAX_VEHICLES - vehicleCount), SeekOrigin.Current);

            var paddingEnd = MAX_LENGTH - br.Position; // 376
            var pad = new List<byte>();
            for (int i = 0; i < paddingEnd; i++)
            {
                pad.Add(br.ReadUint8());
            }

            return new B4VehicleList
            ( 
                vehicleCount, 
                vehicleIsDriveable, 
                raceCarRanks, 
                vehicleIDs, 
                vehicleMaxCrashScore, 
                vehicleGrudgePoints, 
                vehiclePrice, 
                vehicleDefaultColor, 
                pad,
                versionNumber
            );
        }

        public override void Write(BinaryWriterE bw, Models.Common.VList obj)
        {
            if (obj is not B4VehicleList b4Obj)
            {
                throw new ArgumentException("Object is not of type B4VehicleList");
            }

            // Header
            Console.WriteLine($"\nWriting header");
            bw.WriteInt32(b4Obj.VersionNumber);
            bw.WriteInt32(b4Obj.VehicleCount);
            Console.WriteLine($"Finished writing header");

            var count = b4Obj.VehicleCount;

            // IsDrivable
            Console.WriteLine($"\nWriting IsDrivables[{count}]");
            for (int i = 0; i < count; i++)
            {
                Console.WriteLine($"Offset {bw.Position}: Writing IsDrivable[{i}]");
                bw.WriteBool(b4Obj.VehicleIsDriveable[i]);
            }
            Console.WriteLine($"Finished writing IsDrivables");

            // seek forward if necessary
            if (count < MAX_VEHICLES)
                bw.Seek((MAX_VEHICLES - count) * 4, SeekOrigin.Current);

            // RaceCarRanks
            Console.WriteLine($"\nWriting RaceCarRanks[{count}]");
            for (int i = 0; i < count; i++)
            {
                Console.WriteLine($"Offset {bw.Position}: Writing RaceCarRanks[{i}]");
                bw.WriteInt32(b4Obj.RaceCarRanks[i]);
            }
            Console.WriteLine($"Finished writing RaceCarRanks");

            // seek forward if necessary
            if (count < MAX_VEHICLES)
                bw.Seek((MAX_VEHICLES - count) * 4, SeekOrigin.Current);

            // VehicleIDs
            Console.WriteLine($"\nWriting VehicleIDs[{count}]");
            for (int i = 0; i < count; i++)
            {
                Console.WriteLine($"Offset {bw.Position}: Writing VehicleIDs[{i}]");
                bw.WriteUlong(b4Obj.VehicleIDs[i]);
            }
            Console.WriteLine($"Finished writing VehicleIDs");

            // seek forward if necessary
            if (count < MAX_VEHICLES)
                bw.Seek((MAX_VEHICLES - count) * 8, SeekOrigin.Current);

            // VehicleMaxCrashScore
            Console.WriteLine($"\nWriting VehicleMaxCrashScore[{count}]");
            for (int i = 0; i < count; i++)
            {
                Console.WriteLine($"Offset {bw.Position}: Writing VehicleMaxCrashScore[{i}]");
                bw.WriteInt32(b4Obj.VehicleMaxCrashScore[i]);
            }
            Console.WriteLine($"Finished writing VehicleMaxCrashScore");

            // seek forward if necessary
            if (count < MAX_VEHICLES)
                bw.Seek((MAX_VEHICLES - count) * 4, SeekOrigin.Current);

            // VehicleGrudgePoints
            Console.WriteLine($"\nWriting VehicleGrudgePoints[{count}]");
            for (int i = 0; i < count; i++)
            {
                Console.WriteLine($"Offset {bw.Position}: Writing VehicleGrudgePoints[{i}]");
                bw.WriteInt32(b4Obj.VehicleGrudgePoints[i]);
            }
            Console.WriteLine($"Finished writing VehicleGrudgePoints");

            // seek forward if necessary
            if (count < MAX_VEHICLES)
                bw.Seek((MAX_VEHICLES - count) * 4, SeekOrigin.Current);

            // VehiclePrice
            Console.WriteLine($"\nWriting VehiclePrice[{count}]");
            for (int i = 0; i < count; i++)
            {
                Console.WriteLine($"Offset {bw.Position}: Writing VehiclePrice[{i}]");
                bw.WriteInt32(b4Obj.VehiclePrice[i]);
            }
            Console.WriteLine($"Finished writing VehiclePrice");

            // seek forward if necessary
            if (count < MAX_VEHICLES)
                bw.Seek((MAX_VEHICLES - count) * 4, SeekOrigin.Current);

            // VehicleDefaultColor
            Console.WriteLine($"\nWriting VehicleDefaultColor[{count}]");
            for (int i = 0; i < count; i++)
            {
                Console.WriteLine($"Offset {bw.Position}: Writing VehicleDefaultColor[{i}]");
                bw.WriteInt8(b4Obj.VehicleDefaultColor[i]);
            }
            Console.WriteLine($"Finished writing VehicleDefaultColor");

            // seek forward if necessary
            if (count < MAX_VEHICLES)
                bw.Seek((MAX_VEHICLES - count), SeekOrigin.Current);

            // Pad
            var paddingEnd = MAX_LENGTH - bw.Position; // 376
            Console.WriteLine($"\nWriting Pad[{paddingEnd}]");
            for (int i = 0; i < paddingEnd; i++)
            {
                Console.WriteLine($"Offset {bw.Position}: Writing Pad[{i}]");
                bw.WriteUint8(0); // b4Obj.Pad[i]
            }
            Console.WriteLine($"Finished writing Pad");
        }
    }
}