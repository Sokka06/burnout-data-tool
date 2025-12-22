using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bdtool.Binary;
using bdtool.Models.B3;
using bdtool.Models.Common;
using bdtool.Models.Types;
using bdtool.Parsers.VData;

namespace bdtool.Parsers
{
    public class B3VehicleDataParser : VehicleDataParser
    {
        public override VehicleData Read(BinaryReaderE br)
        {
            Console.WriteLine($"Offset {br.Position}: Reading Version...");
            var versionNumber = br.ReadUInt32();
            Console.WriteLine($"Offset {br.Position}: Reading Loaded State...");
            var loadedState = br.ReadInt32();
            Console.WriteLine($"Offset {br.Position}: Reading File Size...");
            var fileSize = br.ReadInt32();
            Console.WriteLine($"Offset {br.Position}: Reading Num Body Parts...");
            var numBodyParts = br.ReadInt8();
            Console.WriteLine($"Offset {br.Position}: Reading Num Wheels...");
            var numWheels = br.ReadInt8();
            Console.WriteLine($"Offset {br.Position}: Reading Min LOD...");
            var minLod = br.ReadInt16();
            Console.WriteLine($"Offset {br.Position}: Reading Max LOD...");
            var maxLod = br.ReadInt16();

            //skip padding
            Console.WriteLine($"Offset {br.Position}: Skipping Padding...");
            br.Seek(2, SeekOrigin.Current);

            Console.WriteLine($"Offset {br.Position}: Reading Object Radius...");
            var objectRadius = br.ReadFloat();
            Console.WriteLine($"Offset {br.Position}: Reading Wheel Radius...");
            var wheelRadius = br.ReadFloat();

            Console.WriteLine($"Offset {br.Position}: Reading Wheel Scales...");
            var wheelScales = new List<float>(6);
            for (int i = 0; i < 6; i++)
            {
                Console.WriteLine($"Offset {br.Position}: Reading Wheel Scale[{i}]...");
                wheelScales.Add(br.ReadFloat());
            }

            Console.WriteLine($"Offset {br.Position}: Reading Body Part Radii...");
            var bodyPartRadii = new List<float>(6);
            for (int i = 0; i < 6; i++)
            {
                Console.WriteLine($"Offset {br.Position}: Reading Body Part Radii[{i}]...");
                bodyPartRadii.Add(br.ReadFloat());
            }

            //skip data for now
            Console.WriteLine($"Offset {br.Position}: Skipping Data...");
            br.Seek(0xB80, SeekOrigin.Begin);
            
            Console.WriteLine($"Offset {br.Position}: Reading Wheel Matrices...");
            var wheelMatrices = new List<Matrix3x4>(6);
            for (int i = 0; i < 6; i++)
            {
                Console.WriteLine($"Offset {br.Position}: Reading Wheel Matrix[{i}]...");
                var matrix = new Matrix3x4() 
                { 
                    Right = br.ReadV3d(),
                    Up = br.ReadV3d(),
                    At = br.ReadV3d(),
                    Pos = br.ReadV3d()
                };
                wheelMatrices.Add(matrix);
            }

            return new B3VehicleData() 
            {
                VersionNumber = versionNumber,
                LoadedState = loadedState,
                FileSize = fileSize,
                NumBodyParts = numBodyParts,
                NumWheels = numWheels,
                MinLOD = minLod,
                MaxLOD = maxLod,
                ObjectRadius = objectRadius,
                WheelRadius = wheelRadius,
                WheelScales = wheelScales,
                BodyPartRadii = bodyPartRadii,
                WheelMatrices = wheelMatrices
            };
        }

        public override void Write(BinaryWriterE bw, VehicleData obj)
        {

        }
    }
}
