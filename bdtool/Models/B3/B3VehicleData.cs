using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bdtool.Models.Common;
using bdtool.Models.Types;

namespace bdtool.Models.B3
{
    public record B3VehicleData : VehicleData
    {
        public int LoadedState { get; init; } // always zero in files
        public int FileSize { get; init; }
        public sbyte NumBodyParts { get; init; }
        public sbyte NumWheels { get; init; }
        public short MinLOD { get; init; }
        public short MaxLOD { get; init; }
        public float ObjectRadius { get; init; }
        public float WheelRadius { get; init; }
        public required List<float> WheelScales { get; init; } // 6
        public required List<float> BodyPartRadii { get; init; } // 6

        // TODO: ADD MISSING DATA

        public required List<Matrix3x4> WheelMatrices { get; init; } // 6


        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.AppendLine($"Version: {VersionNumber}");
            builder.AppendLine($"LoadedState: {LoadedState}");
            builder.AppendLine($"FileSize: {FileSize}");
            builder.AppendLine($"NumBodyParts: {NumBodyParts}");
            builder.AppendLine($"NumWheels: {NumWheels}");
            builder.AppendLine($"MinLOD: {MinLOD}");
            builder.AppendLine($"MaxLOD: {MaxLOD}");
            builder.AppendLine($"ObjectRadius: {ObjectRadius}");
            builder.AppendLine($"WheelRadius: {WheelRadius}");
            for (int i = 0; i < NumWheels; i++)
            {
                builder.AppendLine($"WheelScale[{i}]: {WheelScales[i]}");
            }
            for (int i = 0; i < NumBodyParts; i++)
            {
                builder.AppendLine($"BodyPartRadii[{i}]: {BodyPartRadii[i]}");
            }

            // ADD MISSING DATA

            for (int i = 0; i < NumWheels; i++)
            {
                builder.AppendLine($"WheelMatrices[{i}]: {WheelMatrices[i]}");
            }

            return builder.ToString();
        }
    }
}
