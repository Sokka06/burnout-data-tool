using System;
using System.Collections.Generic;
using System.CommandLine;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bdtool.Binary;
using bdtool.Dto;
using bdtool.Models.Common;
using bdtool.Models.VDB;
using bdtool.Parsers;
using bdtool.Utilities;
using bdtool.Yaml;

namespace bdtool.Commands.VDB
{
    public static class VDBImportCommand
    {
        public static Command Build()
        {
            var cmd = new Command("import", "Build a VDB file from a YAML VDB file created with this tool.");

            var formatOpt = new Option<VDBFormat>("--format", "-f") 
            { 
                Description = "Specifies the format. Raw is a more accurate representation of the file, while DTO (Data Transfer Object) is more readable and easier to edit.", 
                DefaultValueFactory = ParseResult => VDBFormat.Raw 
            };
            var endianOpt = new Option<Utilities.Binary.Endian>("--endian", "-e")
            {
                Description = "Selects the endianness of the created file. Small by default.",
                DefaultValueFactory = parseResult => Utilities.Binary.Endian.Little
            };
            var verboseOpt = new Option<bool>("--verbose", "-v");

            //endian.AcceptOnlyFromAmong(["small", "big"]);

            var pathArg = new Argument<FileInfo>("path")
            {
                Description = "Path to the YAML VDB file."
            };

            var outPathArg = new Argument<string>("out")
            {
                Description = "Path to the output directory."
            };

            //cmd.Options.Add(input);

            cmd.Arguments.Add(pathArg);
            cmd.Arguments.Add(outPathArg);

            cmd.Options.Add(endianOpt);
            cmd.Options.Add(formatOpt);
            cmd.Options.Add(verboseOpt);

            cmd.SetAction(parseResult =>
            {
                var parsedFile = parseResult.GetValue(pathArg);
                if (parsedFile == null || !parsedFile.Exists)
                {
                    ConsoleEx.Error("Input file does not exist.");
                    return 1;
                }

                var parsedOut = parseResult.GetValue(outPathArg);
                if (string.IsNullOrEmpty(parsedOut) || Path.GetDirectoryName(parsedOut) == string.Empty)
                {
                    ConsoleEx.Error($"Output path invalid: '{parsedOut}'");
                    return 1;
                }

                var parsedFormat = parseResult.GetValue(formatOpt);
                var parsedEndian = parseResult.GetValue(endianOpt);

                // Deserialize YAML to VDB object
                var reader = new YamlDeserializer();
                var yamlText = File.ReadAllText(parsedFile.FullName);

                using var vdbFile = File.Create(parsedOut);
                var writer = new BinaryWriterE(vdbFile, parsedEndian);
                var vdbParser = new VDBParser();

                VDBFile vdb;

                // Write VDB file
                ConsoleEx.Info($"\nDeserializing VDB Data to YAML from '{parsedFormat}' format...\n");

                switch (parsedFormat)
                {
                    case VDBFormat.Raw:
                        vdb = reader.Deserialize<VDBFile>(yamlText);
                        break;
                    case VDBFormat.Dto:
                        vdb = Converters.VDBConverter.FromDto(reader.Deserialize<Dto.VDB>(yamlText));
                        break;
                    default:
                        throw new Exception("Something went VERY wrong while figuring out the format.");
                }

                vdbParser.Write(writer, vdb);

                ConsoleEx.Break();
                ConsoleEx.Info($"Total length: {vdbFile.Length} bytes.");
                ConsoleEx.Info($"VDB file saved to '{Path.GetFullPath(parsedOut)}'");
                ConsoleEx.Break();

                return 0;
            });

            return cmd;
        }
    }
}
