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

            var pathArg = new Argument<FileInfo>("path")
            {
                Description = "Path to the VList file."
            };

            var outPathArg = new Argument<string>("out")
            {
                Description = "Path to the output file.",
                DefaultValueFactory = _ => ""
            };
            
            var verboseOpt = new Option<bool>("--verbose", "-v");

            cmd.Arguments.Add(pathArg);
            cmd.Arguments.Add(outPathArg);

            cmd.Options.Add(verboseOpt);

            cmd.SetAction(parseResult =>
            {
                var parsedFile = parseResult.GetValue(pathArg);
                if (parsedFile == null || !parsedFile.Exists)
                {
                    Console.WriteLine("Input file does not exist.");
                    return 1;
                }

                var parsedOut = parseResult.GetValue(outPathArg);
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

                // Peek the first 4 bytes to get endian.
                var headerBytes = new byte[4];
                fs.Read(headerBytes, 0, 4);

                // Detect endian
                var endian = Utilities.Binary.DetectEndian(headerBytes);
                ConsoleEx.Info($"Using '{endian}' endian.");

                var version = BitConverterE.ToInt32(headerBytes, Utilities.Binary.Endian.Little);
                ConsoleEx.Info($"VList Version '{version}'.");

                // Rewind back to start
                fs.Seek(0, SeekOrigin.Begin);

                // Parse VList file and serialize to YAML
                ConsoleEx.Info("Parsing VList Data...");

                var reader = new BinaryReaderE(fs, endian);
                var parser = new Parsers.VList.VListParser();
                var vlist = parser.Read(reader);

                // Serialize to YAML
                ConsoleEx.Info("Serializing to YAML...");

                var serializer = new YamlSerializer();
                var yamlText = serializer.Serialize(Converters.VListConverter.ToDto(vlist));

                ConsoleEx.Info("Writing data to YAML file...");

                // Write YAML to file
                File.WriteAllText(parsedOut, yamlText);

                ConsoleEx.Info($"\nYAML file saved to '{Path.GetFullPath(parsedOut)}'");
                return 0;
            });

            return cmd;
        }
    }
}
