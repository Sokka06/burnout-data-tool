using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using bdtool.Binary;
using bdtool.Models.VDB;

namespace bdtool.Parsers
{
    public class VDBParser : IParser<VDBFile>
    {
        private readonly VDBHeaderParser _headerParser = new();
        private readonly DatabaseDefaultValueParser _defaultValueParser = new();
        private readonly DatabaseValueParser _valueParser = new();
        private readonly DatabaseFileDefParser _defaultFileDefParser = new();

        public VDBFile Read(BinaryReaderE br)
        {
            // Parse Header
            Console.WriteLine($"Offset {br.Position}: Parsing header");
            var header = _headerParser.Read(br);
            Console.WriteLine($"Offset {br.Position}: Finished parsing header");

            // Parse Default Values
            Console.WriteLine($"Offset {br.Position}: Parsing Default Values");
            var defaultValues = new List<DatabaseDefaultValue>(header.DefaultValueCount);
            for (int i = 0; i < header.DefaultValueCount; i++)
            {
                var value = _defaultValueParser.Read(br);
                defaultValues.Add(value);
            }
            Console.WriteLine($"Offset {br.Position}: Finished parsing Default Values");


            /*Console.WriteLine($"Offset {br.Position}: Seeking forward to first vector3 value.");
            var minAddress = br.Position;
            var maxAddress = header.FileDefOffset;

            int current = (int)br.Position;
            int? next = defaultValues
                .Select(v => v.Data.RawValue)
                .Where(v => v >= current && v <= maxAddress)
                .Min();

            if (next.HasValue)
            {
                Console.WriteLine($"Offset {br.Position}: Found first value at {next.Value}.");
                br.Seek(next.Value - current, SeekOrigin.Current);
            }
            else
            {
                Console.WriteLine($"Offset {br.Position}: No values found, skipping data section.");
            }

            

            var values = new List<DatabaseValue>();
            while (br.Position < header.FileDefOffset)
            {
                var value = _valueParser.Read(br);
                values.Add(value);
            }*/

            // The next section contains 4-12 bytes of padding and then vector3 values (with 4 bytes of padding), 
            // the Unk1 value seems to indicate the count of these values.

            // After that, there seems to be a mix of ints and floats until the file defs section.
            // Perhaps they're common or shared values.

            // parse all addresses and raw values
            Console.WriteLine($"Offset {br.Position}: Parsing Values");
            var values = new List<DatabaseValue>();
            while (br.Position < header.FileDefOffset)
            {
                var value = _valueParser.Read(br);
                values.Add(value);
            }

            Console.WriteLine($"Offset {br.Position}: Finished parsing Values");

            if (br.Position != header.FileDefOffset)
            {
                Console.WriteLine($"Offset {br.Position}: Seeking to FileDefOffset");
                br.Seek(header.FileDefOffset, SeekOrigin.Begin);
            }

            // Parse File Definitions
            Console.WriteLine($"Offset {br.Position}: Parsing File Definitions");
            var fileDefs = new List<DatabaseFileDef>(header.FileDefCount);
            for (int i = 0; i < header.FileDefCount; i++)
            {
                var value = _defaultFileDefParser.Read(br);
                fileDefs.Add(value);
            }
            Console.WriteLine($"Offset {br.Position}: Finished parsing File Definitions");

            //Console.WriteLine($"Reader stopped at '{fs.Position}'");

            return new VDBFile 
            { 
                Header = header, 
                DefaultValues = defaultValues, 
                Values = values, 
                FileDefs = fileDefs 
            };
        }

        public void Write(BinaryWriterE bw, VDBFile obj)
        {
            // Write Header
            Console.WriteLine($"Offset {bw.Position}: Writing header");
            _headerParser.Write(bw, obj.Header);
            Console.WriteLine($"Offset {bw.Position}: Finished writing header");

            // Write Default Values
            Console.WriteLine($"Offset {bw.Position}: Writing Default Values");
            for (int i = 0; i < obj.Header.DefaultValueCount; i++)
            {
                Console.WriteLine($"Writing Default Value at address '{bw.Position}'");
                _defaultValueParser.Write(bw, obj.DefaultValues[i]);
            }
            Console.WriteLine($"Offset {bw.Position}: Finished writing Default Values");

            // Skip padding
            //Console.WriteLine($"Offset {bw.Position}: Seeking forward 4 bytes");
            //bw.Seek(4, SeekOrigin.Current);

            // Write Values
            Console.WriteLine($"Offset {bw.Position}: Writing Values");
            for (int i = 0; i < obj.Values.Count; i++)
            {
                //Console.WriteLine($"Writing Value at address '{bw.Position}'");
                _valueParser.Write(bw, obj.Values[i]);
            }
            Console.WriteLine($"Offset {bw.Position}: Finished writing Values");

            // Write File Definitions
            if (bw.Position != obj.Header.FileDefOffset)
            {
                Console.WriteLine($"Offset {bw.Position}: Seeking to FileDefOffset");
                bw.Seek(obj.Header.FileDefOffset, SeekOrigin.Begin);
            }

            Console.WriteLine($"Offset {bw.Position}: Writing File Definitions");
            for (int i = 0; i < obj.FileDefs.Count; i++)
            {
                _defaultFileDefParser.Write(bw, obj.FileDefs[i]);
            }
            Console.WriteLine($"Offset {bw.Position}: Finished parsing File Definitions");
        }
    }
}
