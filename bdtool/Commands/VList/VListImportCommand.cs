using System;
using System.Collections;
using System.Collections.Generic;
using System.CommandLine;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bdtool.Binary;
using bdtool.Models.B3;
using bdtool.Models.B4;
using bdtool.Models.Common;
using bdtool.Parsers;
using bdtool.Utilities;
using bdtool.Yaml;

namespace bdtool.Commands.VList
{
    public static class VListImportCommand
    {
        private static readonly int[] versions = [6, 9];

        public static Command Build()
        {
            var cmd = new Command("import", "Build a VList file from a YAML VList file created with this tool.");
            
            var pathArg = new Argument<FileInfo>("path")
            {
                Description = "Path to the YAML VList file."
            };

            var outPathArg = new Argument<string>("out")
            {
                Description = "Path to the output file."
            };
            
            var verboseOpt = new Option<bool>("--verbose", "-v");
            var endianOpt = new Option<Utilities.Binary.Endian>("--endian", "-e")
            {
                Description = "Selects the endian of the created file. Small by default.",
                DefaultValueFactory = _ => Utilities.Binary.Endian.Little
            };

            cmd.Arguments.Add(pathArg);
            cmd.Arguments.Add(outPathArg);

            cmd.Options.Add(verboseOpt);
            cmd.Options.Add(endianOpt);

            cmd.SetAction(parseResult =>
            {
                var parsedFile = parseResult.GetValue(pathArg);
                if (parsedFile == null || !parsedFile.Exists)
                {
                    Console.WriteLine("Input file does not exist.");
                    return 1;
                }

                var parsedOut = parseResult.GetValue(outPathArg);
                if (string.IsNullOrEmpty(parsedOut) || Path.GetDirectoryName(parsedOut) == string.Empty)
                {
                    Console.WriteLine($"Output path invalid: '{parsedOut}'");
                    return 1;
                }

                var parsedEndian = parseResult.GetValue(endianOpt);

                ConsoleEx.Info("Deserializing VList Data...");

                var yamlText = File.ReadAllText(parsedFile.FullName);
                var deserializer = new YamlDeserializer();
                var vlistDto = deserializer.Deserialize<Dto.VehicleList>(yamlText);

                ConsoleEx.Info($"Writing VList data to a file using '{parsedEndian}'...");

                using var vlistFile = File.Create(parsedOut);
                var writer = new BinaryWriterE(vlistFile, parsedEndian);
                var parser = new Parsers.VList.VListParser();
                parser.Write(writer, Converters.VListConverter.FromDto(vlistDto));
                
                ConsoleEx.Info($"Total length: {vlistFile.Length} bytes.");
                ConsoleEx.Info($"\nVList file saved to '{Path.GetFullPath(parsedOut)}' using '{parsedEndian}' endian!");
                
                return 0;
            });

            return cmd;
        }

    }
}
