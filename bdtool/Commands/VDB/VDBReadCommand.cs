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

            var verboseOpt = new Option<bool>("--verbose", "-v") 
            {
                DefaultValueFactory = ParseResult => false
            };

            var pathArg = new Argument<FileInfo>("path")
            {
                Description = "Path to the VDB file."
            };

            var sectionsOpt = new Option<string>("--sections", "-s")
            {
                Description = "Sections to print (header, defaults, values, defs). Expects section names seperated by a comma, e.g. \"header, defs\".",
                DefaultValueFactory = parseResult => ""
            };

            cmd.Arguments.Add(pathArg);
            cmd.Options.Add(sectionsOpt);

            cmd.Options.Add(verboseOpt);

            cmd.SetAction(parseResult =>
            {
                var parsedFile = parseResult.GetValue(pathArg);
                if (parsedFile == null || !parsedFile.Exists)
                {
                    ConsoleEx.Error($"Input file does not exist at '{parsedFile?.FullName}'.");
                    return 1;
                }

                var parsedVerbose = parseResult.GetValue(verboseOpt);

                using var fs = File.OpenRead(parsedFile.FullName);

                // Peek the first 4 bytes to get endianess.
                var headerBytes = new byte[4];
                fs.Read(headerBytes, 0, 4);

                // Detect Endianness
                var endian = Utilities.Binary.DetectEndian(headerBytes);

                if (parsedVerbose)
                    ConsoleEx.Info($"Using '{endian}' endian.");

                // Rewind back to start
                fs.Seek(0, SeekOrigin.Begin);

                if (parsedVerbose)
                    ConsoleEx.Info("Reading VDB Data...");

                var reader = new BinaryReaderE(fs, endian);

                var vdbParser = new VDBParser();
                var vdbFile = vdbParser.Read(reader);

                ConsoleEx.Break();

                // Check header and warn
                if (vdbFile.Header.Type != 2)
                {
                    ConsoleEx.Warning($"Warning! VDB Header Type is {vdbFile.Header.Type}, data may not be compatible with this tool.");
                }

                // Print sections
                var parsedSections = parseResult.GetValue(sectionsOpt);
                var sections = parsedSections.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToArray();

                if (sections.Length > 0)
                {
                    foreach (var section in sections)
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
