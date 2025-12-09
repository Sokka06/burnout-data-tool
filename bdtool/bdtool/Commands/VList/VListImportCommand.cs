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
        private static readonly int[] versions = new int[] { 6, 9 };

        public static Command Build()
        {
            var cmd = new Command("import", "Build a VList file from a YAML VList file created with this tool.");

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
                Description = "Path to the YAML VList file."
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

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Deserializing VList Data...");
                Console.ResetColor();

                var yamlText = File.ReadAllText(parsedFile.FullName);
                var reader = new YamlDeserializer();
                var vlist = reader.Deserialize<Models.Common.VList>(yamlText);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Writing VList data to a file...");
                Console.ResetColor();

                using var vlistFile = File.Create(parsedOut);
                var writer = new EndianBinaryWriter(vlistFile, parsedEndian);
                var parser = new Parsers.VList.VListParser();
                parser.Write(writer, vlist);
                Console.WriteLine($"Total length: {vlistFile.Length} bytes.");

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\nVList file saved to '{Path.GetFullPath(parsedOut)}'");
                Console.ResetColor();

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
