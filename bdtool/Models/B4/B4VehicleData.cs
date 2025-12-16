using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bdtool.Models.Common;

namespace bdtool.Models.B4
{
    public record B4VehicleData : VehicleData
    {
        public int LoadedState { get; init; } // always zero in files
        public byte NumBodyParts { get; init; }
        public byte NumWheels { get; init; }
        public byte MinLOD { get; init; }
        public byte MaxLOD { get; init; }
        public float ObjectRadius { get; init; }
        public float WheelRadius { get; init; }
        public required List<float> WheelScales { get; init; } // 6
        public required List<float> BodyPartRadii { get; init; } // 8
    }
}
