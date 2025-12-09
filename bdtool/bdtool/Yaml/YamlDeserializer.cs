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

namespace bdtool.Yaml
{
    public class YamlDeserializer
    {
        public T Deserialize<T>(string yaml)
        {
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .WithTagMapping("!B3VehicleList", typeof(B3VehicleList))
                .WithTagMapping("!B4VehicleList", typeof(B4VehicleList))
                .Build();

            //yml contains a string containing your YAML
            var obj = deserializer.Deserialize<T>(yaml);

            return obj;
        }
    }
}
