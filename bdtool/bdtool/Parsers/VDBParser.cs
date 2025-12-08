using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bdtool.Models.Common;
using bdtool.Utilities;

namespace bdtool.Parsers
{
    public class VDBParser : IParser<VDBFile>
    {
        private readonly VDBHeaderParser _headerParser = new();
        private readonly DatabaseDefaultValueParser _defaultValueParser = new();
        private readonly DatabaseValueParser _valueParser = new();
        private readonly DatabaseFileDefParser _defaultFileDefParser = new();

        public VDBFile Parse(EndianBinaryReader br)
        {
            // Parse Header
            Console.WriteLine($"Offset {br.Position}: Parsing header");
            var header = _headerParser.Parse(br);
            Console.WriteLine($"Offset {br.Position}: Finished parsing header");

            // Parse Default Values
            Console.WriteLine($"Offset {br.Position}: Parsing Default Values");
            var defaultValues = new List<DatabaseDefaultValue>(header.DefaultValueCount);
            for (int i = 0; i < header.DefaultValueCount; i++)
            {
                var value = _defaultValueParser.Parse(br);
                defaultValues.Add(value);
            }
            Console.WriteLine($"Offset {br.Position}: Finished parsing Default Values");

            // Parse Values

            Console.WriteLine($"Offset {br.Position}: Seeking forward by 12 bytes");
            br.Seek(12, SeekOrigin.Current);
            Console.WriteLine($"Offset {br.Position}: Parsing Values");
            var values = new List<DatabaseValue>();
            for (int i = 0; i < header.Unk1; i++)
            {
                var value = _valueParser.Parse(br);
                values.Add(value);
            }
            Console.WriteLine($"Offset {br.Position}: Finished parsing Values");

            // Parse File Definitions
            Console.WriteLine($"Offset {br.Position}: Seeking to FileDefOffset");
            br.Seek(header.FileDefOffset, SeekOrigin.Begin);

            Console.WriteLine($"Offset {br.Position}: Parsing File Definitions");
            var fileDefs = new List<DatabaseFileDef>(header.FileDefCount);
            for (int i = 0; i < header.FileDefCount; i++)
            {
                var value = _defaultFileDefParser.Parse(br);
                fileDefs.Add(value);
            }
            Console.WriteLine($"Offset {br.Position}: Finished parsing File Definitions");

            //Console.WriteLine($"Reader stopped at '{fs.Position}'");

            return new VDBFile(header, defaultValues, values, fileDefs);
        }
    }
}
