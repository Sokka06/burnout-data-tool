using System;
using System.Collections.Generic;
using System.CommandLine;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bdtool.Binary;
using bdtool.Parsers;
using bdtool.Utilities;

namespace bdtool.Commands.VDB
{
    public static class VDBReadCommand
    {
        public static Command Build()
        {
            var cmd = new Command("read", "Prints out VDB file data. \nBy default will print all sections, pass an array of sections as the second argument to select which sections to print.");

            //var input = new Option<FileInfo>("--input", "-i") { Required = true, Description = "Path to the VDB file" };
            var verbose = new Option<bool>("--verbose", "-v");

            var path = new Argument<FileInfo>("path")
            {
                Description = "Path to the VDB file."
            };

            var sections = new Argument<string[]>("sections")
            {
                Description = "Sections to print (header, defaults, values, defs). Expects an array of strings, e.g. [\"header\", \"defs\"].",
                DefaultValueFactory = parseResult => []
            };

            var header = new Argument<bool>("header")
            {
                Description = "Print header.",
                DefaultValueFactory = parseResult => true
            };

            var defaultValues = new Argument<bool>("default")
            {
                Description = "Print default values.",
                DefaultValueFactory = parseResult => true
            };

            var values = new Argument<bool>("values")
            {
                Description = "Print values.",
                DefaultValueFactory = parseResult => true
            };

            var fileDefs = new Argument<bool>("defs")
            {
                Description = "Print file defs.",
                DefaultValueFactory = parseResult => true
            };

            //cmd.Options.Add(input);

            cmd.Arguments.Add(path);
            cmd.Arguments.Add(sections);
            //cmd.Arguments.Add(header);
            //cmd.Arguments.Add(defaultValues);
            //cmd.Arguments.Add(values);
            //cmd.Arguments.Add(fileDefs);

            cmd.Options.Add(verbose);

            cmd.SetAction(parseResult =>
            {
                FileInfo? parsedFile = parseResult.GetValue(path);
                if (parsedFile == null || !parsedFile.Exists)
                {
                    Console.WriteLine("Input file does not exist.");
                    return 1;
                }

                using var fs = File.OpenRead(parsedFile.FullName);

                // Peek the first 4 bytes to get endianess.
                byte[] headerBytes = new byte[4];
                fs.Read(headerBytes, 0, 4);

                // Detect Endianness
                var endian = Utilities.Binary.DetectEndianness(headerBytes);
                Console.WriteLine($"Using '{endian}' endian.");

                // Rewind back to start
                fs.Seek(0, SeekOrigin.Begin);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nReading VDB Data...\n");
                Console.ResetColor();

                var reader = new EndianBinaryReader(fs, endian);

                var vdbParser = new VDBParser();
                var vdbFile = vdbParser.Read(reader);

                // Check header and warn
                if (vdbFile.Header.Type != 2)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"\nWarning! VDB Header Type is {vdbFile.Header.Type}, data may not be compatible with this tool.");
                    Console.ResetColor();
                }

                // Print sections
                var parsedHeader = parseResult.GetValue(header);
                var parsedDefaultValues = parseResult.GetValue(defaultValues);
                var parsedValues = parseResult.GetValue(values);
                var parsedFileDefs = parseResult.GetValue(fileDefs);
                var parsedSections = parseResult.GetValue(sections);

                if (parsedSections != null && parsedSections.Length > 0)
                {
                    foreach (var section in parsedSections)
                    {
                        switch (section.ToLower())
                        {
                            case "header":
                                Console.WriteLine(vdbFile.PrintHeader());
                                break;
                            case "defaults":
                                Console.WriteLine(vdbFile.PrintDefaultValues());
                                break;
                            case "values":
                                Console.WriteLine(vdbFile.PrintValues());
                                break;
                            case "defs":
                                Console.WriteLine(vdbFile.PrintFileDefs());
                                break;
                            default:
                                break;
                        }
                    }
                } 
                else
                {
                    Console.WriteLine(vdbFile.PrintHeader());
                    Console.WriteLine(vdbFile.PrintDefaultValues());
                    Console.WriteLine(vdbFile.PrintValues());
                    Console.WriteLine(vdbFile.PrintFileDefs());
                }

                return 0;
            });

            return cmd;
        }
    }
}
