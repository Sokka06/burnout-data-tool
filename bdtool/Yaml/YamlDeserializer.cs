using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bdtool.Models.B3;
using bdtool.Models.B4;
using YamlDotNet.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization.NodeDeserializers;
using static bdtool.Dto.VDB;

namespace bdtool.Yaml
{
    public class YamlDeserializer
    {
        public T Deserialize<T>(string yaml)
        {
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .WithTagMapping("!B3VehicleList", typeof(Dto.B3.B3VehicleList))
                .WithTagMapping("!B4VehicleList", typeof(Dto.B4.B4VehicleList))
                /*.WithTypeDiscriminatingNodeDeserializer((o) =>
                {
                    IDictionary<string, Type> valueMappings = new Dictionary<string, Type>
                    {
                        { "IntValue", typeof(IntValue) },
                        { "BoolValue", typeof(BoolValue) },
                        { "FloatValue", typeof(FloatValue) },
                        { "PointerValue", typeof(PointerValue) },
                        { "Vector3Value", typeof(Vector3Value) }
                    };
                    o.AddKeyValueTypeDiscriminator<DataValue>("DataValue", valueMappings); // "ObjectType" must match the name of the key exactly as it appears in the Yaml document.
                })*/
                .WithTypeConverter(new DataValueYamlConverter())
                //.WithNodeDeserializer(new DataValueDeserializer(), s => s.InsteadOf<ObjectNodeDeserializer>())
                .Build();

            //yml contains a string containing your YAML
            var obj = deserializer.Deserialize<T>(yaml);

            return obj;
        }
    }
}
