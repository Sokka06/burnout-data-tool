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
            var versionNumber = br.ReadUInt32();
            var loadedState = br.ReadInt32();
            var fileSize = br.ReadInt32();
            var numBodyParts = br.ReadInt8();
            var numWheels = br.ReadInt8();
            var minLod = br.ReadInt16();
            var maxLod = br.ReadInt16();

            //skip padding
            br.Seek(2, SeekOrigin.Current);

            var objectRadius = br.ReadFloat();
            var wheelRadius = br.ReadFloat();

            var wheelScales = new List<float>(6);
            for (int i = 0; i < 6; i++)
            {
                wheelScales.Add(br.ReadFloat());
            }

            var bodyPartRadii = new List<float>(6);
            for (int i = 0; i < 6; i++)
            {
                bodyPartRadii.Add(br.ReadFloat());
            }

            //skip data for now
            br.Seek(0xB80, SeekOrigin.Begin);
            var wheelMatrices = new List<Matrix3x4>(6);
            for (int i = 0; i < 6; i++)
            {
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
