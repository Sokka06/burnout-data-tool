using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bdtool.Dto;
using bdtool.Models.Types;
using bdtool.Parsers;
using YamlDotNet.Core;
using YamlDotNet.Serialization;
using static bdtool.Dto.VDB;

namespace bdtool.Yaml
{
    public sealed class DataValueDeserializer : INodeDeserializer
    {

        public bool Deserialize(IParser reader, Type expectedType, Func<IParser, Type, object?> nestedObjectDeserializer, out object? value, ObjectDeserializer rootDeserializer)
        {
            value = null;

            if (!typeof(DataValue).IsAssignableFrom(expectedType))
                return false;

            // Read the mapping into a dictionary first
            var temp = (Dictionary<string, object?>)
                nestedObjectDeserializer(reader, typeof(Dictionary<string, object?>))!;

            if (!temp.TryGetValue("Type", out var typeObj))
                throw new YamlException("Missing 'Type' field");

            var type = Enum.Parse<DataType>(typeObj!.ToString()!, ignoreCase: true);

            value = type switch
            {
                DataType.RwReal => new FloatValue
                {
                    Type = type,
                    Value = Convert.ToSingle(temp["Value"])
                },

                DataType.RwInt32 => new IntValue
                {
                    Type = type,
                    Value = Convert.ToInt32(temp["Value"])
                },

                DataType.Pointer => new PointerValue
                {
                    Type = type,
                    Address = Convert.ToInt64(temp["Value"])
                },

                DataType.RwBool => new BoolValue
                {
                    Type = type,
                    Value = Convert.ToBoolean(temp["Value"])
                },

                DataType.V3d => new Vector3Value
                {
                    Type = type,
                    Value = (V3d)temp["Value"]!
                },

                _ => throw new YamlException($"Unsupported DataType: {type}")
            };

            return true;
        }
    }
}
