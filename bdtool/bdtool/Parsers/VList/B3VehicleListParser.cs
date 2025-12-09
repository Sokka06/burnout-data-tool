using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bdtool.Binary;
using bdtool.Models.B3;
using bdtool.Models.B4;

namespace bdtool.Parsers.VList
{
    public class B3VehicleListParser : VListParser
    {
        private const int MAX_VEHICLES = 128;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="br"></param>
        /// <returns></returns>
        public override B3VehicleList Read(EndianBinaryReader br)
        {
            Console.WriteLine($"\nReading header");
            var versionNumber = br.ReadInt32();
            var vehicleCount = br.ReadInt32();
            Console.WriteLine($"Finished reading header");

            var padding = MAX_VEHICLES - vehicleCount;

            // IsDrivable
            Console.WriteLine($"\nReading IsDrivables[{vehicleCount}]");
            var vehicleIsDriveable = new List<bool>();
            for (int i = 0; i < vehicleCount; i++)
            {
                Console.WriteLine($"Offset {br.Position}: Reading IsDrivable[{i}]");
                vehicleIsDriveable.Add(br.ReadBool());
            }
            Console.WriteLine($"Finished reading IsDrivables");

            // seek forward if necessary
            if (vehicleCount < MAX_VEHICLES)
                br.Seek(padding * 4, SeekOrigin.Current);

            // RaceCarRanks
            Console.WriteLine($"\nReading RaceCarRanks[{vehicleCount}]");
            var raceCarRanks = new List<int>();
            for (int i = 0; i < vehicleCount; i++)
            {
                Console.WriteLine($"Offset {br.Position}: Reading RaceCarRanks[{i}]");
                raceCarRanks.Add(br.ReadInt32());
            }
            Console.WriteLine($"Finished reading RaceCarRanks");

            // seek forward if necessary
            if (vehicleCount < MAX_VEHICLES)
                br.Seek(padding * 4, SeekOrigin.Current);

            // VehicleIDs
            Console.WriteLine($"\nReading VehicleIDs[{vehicleCount}]");
            var vehicleIDs = new List<ulong>();
            for (int i = 0; i < vehicleCount; i++)
            {
                Console.WriteLine($"Offset {br.Position}: Reading VehicleIDs[{i}]");
                vehicleIDs.Add(br.ReadUlong());
            }
            Console.WriteLine($"Finished reading VehicleIDs");

            // seek forward if necessary
            if (vehicleCount < MAX_VEHICLES)
                br.Seek(padding * 8, SeekOrigin.Current);

            // Unk1
            Console.WriteLine($"\nReading Unk1[{vehicleCount}]");
            var unk1 = new List<int>();
            for (int i = 0; i < vehicleCount; i++)
            {
                Console.WriteLine($"Offset {br.Position}: Reading Unk1[{i}]");
                unk1.Add(br.ReadInt32());
            }
            Console.WriteLine($"Finished reading Unk1");

            // seek forward if necessary
            if (vehicleCount < MAX_VEHICLES)
                br.Seek(padding * 4, SeekOrigin.Current);

            // Unk2
            Console.WriteLine($"\nReading Unk2[{vehicleCount}]");
            var unk2 = new List<int>();
            for (int i = 0; i < vehicleCount; i++)
            {
                Console.WriteLine($"Offset {br.Position}: Reading Unk2[{i}]");
                unk2.Add(br.ReadInt32());
            }
            Console.WriteLine($"Finished reading Unk2");

            // seek forward if necessary
            if (vehicleCount < MAX_VEHICLES)
                br.Seek(padding * 4, SeekOrigin.Current);

            // Pad
            Console.WriteLine($"\nReading Pad[{1016}]");
            var pad = new List<byte>();
            for (int i = 0; i < 1016; i++)
            {
                Console.WriteLine($"Offset {br.Position}: Reading Pad[{i}]");
                pad.Add(br.ReadUint8());
            }
            Console.WriteLine($"Finished reading Pad");

            return new B3VehicleList(vehicleCount, vehicleIsDriveable, raceCarRanks, vehicleIDs, unk1, unk2, pad, versionNumber);
        }

        public override void Write(EndianBinaryWriter bw, Models.Common.VList obj)
        {
            // TODO: Implement Write method
            if (obj is not B3VehicleList b3Obj)
            {
                throw new ArgumentException("Object is not of type B3VehicleList");
            }

            Console.WriteLine($"\nWriting header");
            bw.WriteInt32(b3Obj.VersionNumber);
            bw.WriteInt32(b3Obj.VehicleCount);
            Console.WriteLine($"Finished writing header");

            var count = b3Obj.VehicleCount;
            var padding = MAX_VEHICLES - count;

            // IsDrivable
            Console.WriteLine($"\nWriting IsDrivables[{count}]");
            for (int i = 0; i < count; i++)
            {
                Console.WriteLine($"Offset {bw.Position}: Writing IsDrivable[{i}]");
                bw.WriteBool(b3Obj.VehicleIsDriveable[i]);
            }
            Console.WriteLine($"Finished writing IsDrivables");

            // seek forward if necessary
            if (count < MAX_VEHICLES)
                bw.Seek(padding * 4, SeekOrigin.Current);

            // RaceCarRanks
            Console.WriteLine($"\nWriting RaceCarRanks[{count}]");
            for (int i = 0; i < count; i++)
            {
                Console.WriteLine($"Offset {bw.Position}: Writing RaceCarRanks[{i}]");
                bw.WriteInt32(b3Obj.RaceCarRanks[i]);
            }
            Console.WriteLine($"Finished writing RaceCarRanks");

            // seek forward if necessary
            if (count < MAX_VEHICLES)
                bw.Seek(padding * 4, SeekOrigin.Current);

            // VehicleIDs
            Console.WriteLine($"\nWriting VehicleIDs[{count}]");
            for (int i = 0; i < count; i++)
            {
                Console.WriteLine($"Offset {bw.Position}: Writing VehicleIDs[{i}]");
                bw.WriteUlong(b3Obj.VehicleIDs[i]);
            }
            Console.WriteLine($"Finished writing VehicleIDs");

            // seek forward if necessary
            if (count < MAX_VEHICLES)
                bw.Seek(padding * 8, SeekOrigin.Current);

            // Unk1
            Console.WriteLine($"\nWriting Unk1[{count}]");
            var unk1 = new List<int>();
            for (int i = 0; i < count; i++)
            {
                Console.WriteLine($"Offset {bw.Position}: Writing Unk1[{i}]");
                bw.WriteInt32(b3Obj.Unk1[i]);
            }
            Console.WriteLine($"Finished writing Unk1");

            // seek forward if necessary
            if (count < MAX_VEHICLES)
                bw.Seek(padding * 4, SeekOrigin.Current);

            // Unk2
            Console.WriteLine($"\nWriting Unk2[{count}]");
            for (int i = 0; i < count; i++)
            {
                Console.WriteLine($"Offset {bw.Position}: Writing Unk2[{i}]");
                bw.WriteInt32(b3Obj.Unk2[i]);
            }
            Console.WriteLine($"Finished writing Unk2");

            // seek forward if necessary
            if (count < MAX_VEHICLES)
                bw.Seek(padding * 4, SeekOrigin.Current);

            // Pad
            Console.WriteLine($"\nWriting Pad[{1016}]");
            var pad = new List<byte>();
            for (int i = 0; i < 1016; i++)
            {
                Console.WriteLine($"Offset {bw.Position}: Writing Pad[{i}]");
                bw.WriteUint8(b3Obj.Pad[i]);
            }
            Console.WriteLine($"Finished writing Pad");
        }
    }
}
