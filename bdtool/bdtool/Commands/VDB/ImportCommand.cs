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

namespace bdtool.Commands.VDB
{
    public static class ImportCommand
    {
        public static Command Build()
        {
            var cmd = new Command("import", "Build a VDB file from a YAML VDB file created with this tool.");

            //var input = new Option<FileInfo>("--input", "-i") { Required = true, Description = "Path to the VDB file" };
            var verbose = new Option<bool>("--verbose", "-v");
            var endian = new Option<string>("--endian", "-e")
            {
                Description = "Selects the endianness of the created file. Big by default.",
                DefaultValueFactory = parseResult => "big"
            };

            endian.AcceptOnlyFromAmong(["small", "big"]);

            var path = new Argument<FileInfo>("path")
            {
                Description = "Path to the YAML VDB file."
            };

            var outPath = new Argument<string>("out")
            {
                Description = "Path to the output directory."
            };

            //cmd.Options.Add(input);

            cmd.Arguments.Add(path);
            cmd.Arguments.Add(outPath);

            cmd.Options.Add(verbose);
            cmd.Options.Add(endian);

            cmd.SetAction(parseResult =>
            {
                FileInfo? parsedFile = parseResult.GetValue(path);
                if (parsedFile == null || !parsedFile.Exists)
                {
                    Console.WriteLine("Input file does not exist.");
                    return 1;
                }

                string? parsedOut = parseResult.GetValue(outPath);
                if (string.IsNullOrEmpty(parsedOut) || Path.GetDirectoryName(parsedOut) == string.Empty)
                {
                    Console.WriteLine($"Output path invalid: '{parsedOut}'");
                    return 1;
                }

                var parsedEndian = parseResult.GetValue(endian) == "small" ? Utilities.Binary.Endianness.Small : Utilities.Binary.Endianness.Big;

                //using var fs = File.OpenRead(parsedFile.FullName);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nDeserializing VDB Data...\n");
                Console.ResetColor();

                var reader = new YamlDeserializer();

                var yamlText = File.ReadAllText(parsedFile.FullName);
                var vdbObject = reader.Deserialize<VDBFile>(yamlText);

                var vdbFile = File.Create(parsedOut);
                var writer = new EndianBinaryWriter(vdbFile, parsedEndian);
                var vdbParser = new VDBParser();
                vdbParser.Write(writer, vdbObject);

                return 0;
            });

            return cmd;
        }
    }
}
