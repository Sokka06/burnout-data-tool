using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bdtool.Models.B3;
using bdtool.Models.B4;
using bdtool.Models.VDB;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.EventEmitters;
using YamlDotNet.Serialization.NamingConventions;

namespace bdtool.Yaml
{
    class FlowStyleIntegerSequences : ChainedEventEmitter
    {
        public FlowStyleIntegerSequences(IEventEmitter nextEmitter)
            : base(nextEmitter) { }

        public override void Emit(SequenceStartEventInfo eventInfo, IEmitter emitter)
        {
            if (typeof(IEnumerable<DatabaseValue>).IsAssignableFrom(eventInfo.Source.Type))
            {
                eventInfo = new SequenceStartEventInfo(eventInfo.Source)
                {
                    Style = SequenceStyle.Flow
                };
            }

            if (typeof(IEnumerable<DataElement>).IsAssignableFrom(eventInfo.Source.Type))
            {
                eventInfo = new SequenceStartEventInfo(eventInfo.Source)
                {
                    Style = SequenceStyle.Flow
                };
            }

            nextEmitter.Emit(eventInfo, emitter);
        }
    }

    public class YamlSerializer
    {
        public string Serialize<T>(T obj)
        {
            var serializer = new SerializerBuilder()
                .WithTagMapping("!B3VehicleList", typeof(Dto.B3.B3VehicleList))
                .WithTagMapping("!B4VehicleList", typeof(Dto.B4.B4VehicleList))
                //.WithTypeConverter(new DataValueYamlConverter())
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .WithIndentedSequences()
                .DisableAliases()
                //.WithEventEmitter(next => new FlowStyleIntegerSequences(next))
                .ConfigureDefaultValuesHandling(DefaultValuesHandling.Preserve)
                .Build();
            var yaml = serializer.Serialize(obj);

            return yaml;
        }
    }
}
