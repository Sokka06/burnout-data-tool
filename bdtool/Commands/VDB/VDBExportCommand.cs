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
            var verboseOpt = new Option<bool>("--verbose", "-v");

            var pathArg = new Argument<FileInfo>("path")
            {
                Description = "Path to the VDB file."
            };

            var outPathArg = new Argument<string>("out")
            {
                Description = "Path to the output directory."
            };
            
            cmd.Arguments.Add(pathArg);
            cmd.Arguments.Add(outPathArg);

            cmd.Options.Add(definitionsOpt);
            //cmd.Options.Add(paddingOpt);
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
                var parsedDefinitions = parseResult.GetValue(definitionsOpt);
                /*if (parsedDefinitions != null && !parsedDefinitions.Exists)
                {
                    Console.WriteLine("Input file does not exist.");
                    return 1;
                }*/
                
                ConsoleEx.Break();

                using var fs = File.OpenRead(parsedFile.FullName);

                // Peek the first 4 bytes to get endianess.
                var headerBytes = new byte[4];
                fs.Read(headerBytes, 0, 4);

                // Detect endian
                var endian = Utilities.Binary.DetectEndian(headerBytes);
                ConsoleEx.Info($"Using '{endian}' endian.");

                // Rewind back to start
                fs.Seek(0, SeekOrigin.Begin);

                ConsoleEx.Info("Parsing VDB Data...");
                ConsoleEx.Break();

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
                        var definitions = new DatabaseValueDefinitions { DefaultValues = [], Values = [], Files = [] };
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
