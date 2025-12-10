using System;
using System.Collections.Generic;
using System.CommandLine;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bdtool.Binary;
using bdtool.Models.Common;
using bdtool.Parsers;
using bdtool.Utilities;
using bdtool.Yaml;

namespace bdtool.Commands.VList
{
    public static class VListExportCommand
    {
        public static Command Build()
        {
            var cmd = new Command("export", "Exports a VList file to a YAML file.");

            //var input = new Option<FileInfo>("--input", "-i") { Required = true, Description = "Path to the VDB file" };
            var verbose = new Option<bool>("--verbose", "-v");

            var path = new Argument<FileInfo>("path")
            {
                Description = "Path to the VList file."
            };

            var outPath = new Argument<string>("out")
            {
                Description = "Path to the output directory.",
                DefaultValueFactory = ParseResult => ""
            };

            //cmd.Options.Add(input);

            cmd.Arguments.Add(path);
            cmd.Arguments.Add(outPath);

            cmd.Options.Add(verbose);

            cmd.SetAction(parseResult =>
            {
                FileInfo? parsedFile = parseResult.GetValue(path);
                if (parsedFile == null || !parsedFile.Exists)
                {
                    Console.WriteLine("Input file does not exist.");
                    return 1;
                }

                string? parsedOut = parseResult.GetValue(outPath);
                if (string.IsNullOrEmpty(parsedOut))
                {
                    parsedOut = Path.ChangeExtension(parsedFile.FullName, "yaml");
                }

                if (Path.GetDirectoryName(parsedOut) == string.Empty)
                {
                    Console.WriteLine($"Output path invalid: '{parsedOut}'");
                    return 1;
                }

                using var fs = File.OpenRead(parsedFile.FullName);

                // Peek the first 4 bytes to get endianess.
                byte[] headerBytes = new byte[4];
                fs.Read(headerBytes, 0, 4);

                // Detect Endianness
                var endian = Utilities.Binary.DetectEndianness(headerBytes);
                Console.WriteLine($"Using '{endian}' endian.");

                var version = BitConverter.ToInt32(endian == Utilities.Binary.Endianness.Small ? headerBytes : Utilities.Binary.Reverse(ref headerBytes), 0);
                Console.WriteLine($"VList Version '{version}'.");

                // Rewind back to start
                fs.Seek(0, SeekOrigin.Begin);

                // Parse VList file and serialize to YAML
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Parsing VList Data...");
                Console.ResetColor();

                var reader = new BinaryReaderE(fs, endian);
                var parser = new Parsers.VList.VListParser();
                var vlist = parser.Read(reader);

                // Serialize to YAML
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Serializing to YAML...");
                Console.ResetColor();

                var serializer = new YamlSerializer();
                var yamlText = serializer.Serialize(Converters.VListConverter.ToDto(vlist));

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Writing data to YAML file...");
                Console.ResetColor();

                // Write YAML to file
                File.WriteAllText(parsedOut, yamlText);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\nYAML file saved to '{Path.GetFullPath(parsedOut)}'");
                Console.ResetColor();

                return 0;
            });

            return cmd;
        }
    }
}
