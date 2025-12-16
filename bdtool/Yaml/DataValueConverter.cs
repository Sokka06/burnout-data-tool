
using System.Drawing;
using System.Globalization;
using bdtool.Dto;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.RepresentationModel;
using YamlDotNet.Serialization;
using static bdtool.Dto.VDB;

namespace bdtool.Yaml
{

    public sealed class DataValueYamlConverter : IYamlTypeConverter
    {
        public bool Accepts(Type type) => type == typeof(DataValue);

        public object? ReadYaml(IParser parser, Type type, ObjectDeserializer rootDeserializer)
        {
            // Somewhat hacky way to manually deserialize different DataValue types.
            // if there's a better way, please let me know.

            parser.Consume<MappingStart>();

            parser.Consume<Scalar>(); // skip key
            DataType dataType = Enum.Parse<DataType>(parser.Consume<Scalar>().Value);
            //Console.WriteLine($"this is a {dataType}!");
            DataValue? dataValue = default;

            parser.Consume<Scalar>(); // skip key
            switch (dataType)
            {
                case DataType.RwReal:
                    {
                        var floatValue = float.Parse(parser.Consume<Scalar>().Value, CultureInfo.InvariantCulture);
                        //var value = float.TryParse("0f", out var floatValue);
                        dataValue = new FloatValue { Type = dataType, Value = floatValue };
                        break;
                    }
                case DataType.RwBool:
                    {
                        var value = bool.Parse(parser.Consume<Scalar>().Value);
                        dataValue = new BoolValue { Type = dataType, Value = value };
                        break;
                    }
                case DataType.V3d:
                    {
                        parser.Consume<MappingStart>();
                        parser.Consume<Scalar>(); //
                        var x = float.Parse(parser.Consume<Scalar>().Value, CultureInfo.InvariantCulture);
                        parser.Consume<Scalar>(); //
                        var y = float.Parse(parser.Consume<Scalar>().Value, CultureInfo.InvariantCulture);
                        parser.Consume<Scalar>(); //
                        var z = float.Parse(parser.Consume<Scalar>().Value, CultureInfo.InvariantCulture);
                        parser.Consume<Scalar>(); //
                        var p = float.Parse(parser.Consume<Scalar>().Value, CultureInfo.InvariantCulture);
                        parser.Consume<MappingEnd>();

                        dataValue = new Vector3Value { Type = dataType, Value = new Models.Types.V3d { X = x, Y = y, Z = z, Padding = p, } };
                        break;
                    }
                case DataType.Pointer:
                    {
                        var value = long.Parse(parser.Consume<Scalar>().Value);
                        dataValue = new PointerValue { Type = dataType, Address = value };
                        break;
                    }
                default:
                    {
                        var value = int.Parse(parser.Consume<Scalar>().Value);
                        dataValue = new IntValue { Type = dataType, Value = value };
                    }
                    break;
            }

            parser.Consume<MappingEnd>();
            return dataValue;
        }

        public void WriteYaml(IEmitter emitter, object? value, Type type, ObjectSerializer serializer)
        {
            if (value == null) return;

            // Serialize as the concrete type
            serializer(value, value.GetType());
        }
    }
}
