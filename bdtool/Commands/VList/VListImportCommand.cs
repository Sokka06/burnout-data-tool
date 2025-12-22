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

            //var input = new Option<FileInfo>("--input", "-i") { Required = true, Description = "Path to the VDB file" };
            var verboseOpt = new Option<bool>("--verbose", "-v");
            var endianOpt = new Option<Utilities.Binary.Endian>("--endian", "-e")
            {
                Description = "Selects the endianness of the created file. Small by default.",
                DefaultValueFactory = parseResult => Utilities.Binary.Endian.Little
            };

            //endianOpt.AcceptOnlyFromAmong("little", "big");

            var pathArg = new Argument<FileInfo>("path")
            {
                Description = "Path to the YAML VList file."
            };

            var outPathArg = new Argument<string>("out")
            {
                Description = "Path to the output directory."
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

                /*foreach (var version in versions)
                {
                    var hasDeserialized = false;
                    switch (version)
                    {
                        case 6:
                            try
                            {
                                // Try to deserialize YAML to VList object
                                var vlist = reader.Deserialize<B3VehicleList>(yamlText);
                                using var vlistFile = File.Create(parsedOut);
                                var writer = new EndianBinaryWriter(vlistFile, parsedEndian);
                                var parser = new B3VehicleListParser();
                                parser.Write(writer, vlist);
                                hasDeserialized = true;
                            }
                            catch (Exception e)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine(e);
                                Console.ResetColor();
                                return 0;
                                throw;
                            }
                            break;
                        case 9:
                            try
                            {
                                // Try to deserialize YAML to VList object
                                var vlist = reader.Deserialize<B4VehicleList>(yamlText);
                                using var vlistFile = File.Create(parsedOut);
                                var writer = new EndianBinaryWriter(vlistFile, parsedEndian);
                                var parser = new B4VehicleListParser();
                                parser.Write(writer, vlist);
                                hasDeserialized = true;
                            }
                            catch (Exception e)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine(e);
                                Console.ResetColor();
                                return 0;
                                throw;
                            }
                            break;
                        default:
                            break;
                    }

                    if (hasDeserialized)
                        break;
                }*/

                return 0;
            });

            return cmd;
        }

    }
}
