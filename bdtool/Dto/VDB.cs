using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bdtool.Models.Types;
using YamlDotNet.Serialization;

namespace bdtool.Dto
{
    public record VDB
    {
        public int Type { get; init; }
        public int Unk1 { get; init; }
        public required List<DefaultValue> DefaultValues { get; init; }
        public required List<Value> Values { get; init; }
        public required List<FileDef> FileDefs { get; init; }

        /*public record Header()
        {
            public int Type { get; init; }
        }*/

        public record DefaultValue()
        {
            public int NameHash { get; init; }
            public required string Name { get; init; }
            public required string Path { get; init; }
            public required string FileName { get; init; }
            public required DataValue Value { get; init; }
        }

        public record Value()
        {
            public long Address { get; init; }
            public int RawValue { get; init; }
            public required DataValue Data { get; init; }
        }

        public record FloatValue : DataValue
        {
            public float Value { get; init; }
        }

        public record IntValue : DataValue
        {
            public int Value { get; init; }
        }

        public record PointerValue : DataValue
        {
            public long Address { get; init; }
        }

        public record BoolValue : DataValue
        {
            public bool Value { get; init; }
        }

        public record Vector3Value : DataValue
        {
            public required V3d Value { get; init; }
        }

        public abstract record DataValue
        {
            [YamlMember(Order = -1)]
            public DataType Type { get; init; }
        }

        public record FileDef()
        {
            public int NameHash { get; init; }
            public required string FileName { get; init; }
            public bool IsActive { get; init; }
        }
    }
}
