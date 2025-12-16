using System;
using System.Collections.Generic;
using System.CommandLine;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bdtool.Binary;
using bdtool.Converters;
using bdtool.Definitions;
using bdtool.Dto;
using bdtool.Parsers;
using bdtool.Utilities;
using bdtool.Yaml;

namespace bdtool.Commands.VDB
{
    public enum VDBFormat
    {
        Raw,
        Dto
    }

    public static class VDBExportCommand
    {
        public static Command Build()
        {
            var cmd = new Command("export", "Exports a VDB file to a YAML file.");

            var definitionsOpt = new Option<string>("--definitions", "-d") { Description = "", DefaultValueFactory = ParseResult => "" };
            //var paddingOpt = new Option<int>("--padding", "-p") { Description = "Adjusts the padding length in bytes after Default values are parsed. The amount varies per game and platform, try 4 or 12. If some values seem to be missing, adjust in increments of 4.", DefaultValueFactory = ParseResult => 4 };
            var formatOpt = new Option<VDBFormat>("--format", "-f") { Description = "Specifies the format. Raw is a more accurate representation of the file, while DTO (Data Transfer Object) is more readable and easier to edit.", DefaultValueFactory = ParseResult => VDBFormat.Raw };
            var verbose = new Option<bool>("--verbose", "-v");

            var path = new Argument<FileInfo>("path")
            {
                Description = "Path to the VDB file."
            };

            var outPath = new Argument<string>("out")
            {
                Description = "Path to the output directory."
            };

            //cmd.Options.Add(input);

            //var raw = new Command("raw", "Export VDB in raw format.");

            //cmd.Subcommands.Add(raw);

            cmd.Arguments.Add(path);
            cmd.Arguments.Add(outPath);

            cmd.Options.Add(definitionsOpt);
            //cmd.Options.Add(paddingOpt);
            cmd.Options.Add(formatOpt);
            cmd.Options.Add(verbose);

            cmd.SetAction(parseResult =>
            {
                FileInfo? parsedFile = parseResult.GetValue(path);
                if (parsedFile == null || !parsedFile.Exists)
                {
                    ConsoleEx.Error("Input file does not exist.");
                    return 1;
                }

                string? parsedOut = parseResult.GetValue(outPath);
                if (string.IsNullOrEmpty(parsedOut) || Path.GetDirectoryName(parsedOut) == string.Empty)
                {
                    ConsoleEx.Error($"Output path invalid: '{parsedOut}'");
                    return 1;
                }

                var parsedFormat = parseResult.GetValue(formatOpt);
                var parsedDefinitions = parseResult.GetValue(definitionsOpt);
                /*if (parsedDefinitions != null && !parsedDefinitions.Exists)
                {
                    Console.WriteLine("Input file does not exist.");
                    return 1;
                }*/

                using var fs = File.OpenRead(parsedFile.FullName);

                // Peek the first 4 bytes to get endianess.
                byte[] headerBytes = new byte[4];
                fs.Read(headerBytes, 0, 4);

                // Detect Endianness
                var endian = Utilities.Binary.DetectEndianness(headerBytes);
                ConsoleEx.Info($"\nUsing '{endian}' endian.");

                // Rewind back to start
                fs.Seek(0, SeekOrigin.Begin);

                ConsoleEx.Info("Parsing VDB Data...\n");

                // Parse VDB file
                var reader = new BinaryReaderE(fs, endian);
                var vdbParser = new VDBParser();
                var vdbFile = vdbParser.Read(reader);

                var yamlText = "";
                var serializer = new YamlSerializer();

                ConsoleEx.Break();

                switch (parsedFormat)
                {
                    case VDBFormat.Raw:
                        ConsoleEx.Info($"Serializing VDB Data to YAML in '{parsedFormat}' format...");

                        // Serialize to YAML
                        yamlText = serializer.Serialize(vdbFile);
                        break;
                    case VDBFormat.Dto:
                        var definitions = new DatabaseValueDefinitions() { DefaultValues = [], Values = [], Files = [] };
                        if (!string.IsNullOrEmpty(parsedDefinitions))
                        {
                            ConsoleEx.Info($"Loading definitions from '{parsedDefinitions}'...");
                            try
                            {
                                var definitionsText = File.ReadAllText(parsedDefinitions);
                                definitions = new YamlDeserializer().Deserialize<DatabaseValueDefinitions>(definitionsText);
                            }
                            catch (Exception)
                            {
                                ConsoleEx.Error("Failed to deserialize Definitions YAML!");
                                //Console.WriteLine("Failed to deserialize Definitions YAML!");
                                throw;
                            }
                        }

                        ConsoleEx.Info($"Serializing VDB Data to YAML in '{parsedFormat}' format with definitions for 'defaultValues: {definitions.DefaultValues.Count}, values: {definitions.Values.Count}, files: {definitions.Files.Count}'...");
                        yamlText = serializer.Serialize(VDBConverter.ToDto(vdbFile, definitions));
                        break;
                    default:
                        break;
                }

                ConsoleEx.Info("Writing YAML data to file...");

                // Write YAML to file
                File.WriteAllText(parsedOut, yamlText);

                ConsoleEx.Info($"YAML file saved to '{Path.GetFullPath(parsedOut)}'\n");

                return 0;
            });

            return cmd;
        }
    }
}
