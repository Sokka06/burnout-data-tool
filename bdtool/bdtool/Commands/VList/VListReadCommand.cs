using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bdtool.Parsers;
using bdtool.Utilities;

namespace bdtool.Commands.VList
{
    public static class VListReadCommand
    {
        public static Command Build()
        {
            var cmd = new Command("read", "Prints out VList file data. By default prints all sections, pass bool arguments to print only some sections.");

            //var input = new Option<FileInfo>("--input", "-i") { Required = true, Description = "Path to the VList file" };
            var verbose = new Option<bool>("--verbose", "-v");

            var path = new Argument<FileInfo>("path")
            {
                Description = "Path to the VList file."
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
            cmd.Options.Add(verbose);

            cmd.Arguments.Add(path);
            cmd.Arguments.Add(header);
            cmd.Arguments.Add(defaultValues);
            cmd.Arguments.Add(values);
            cmd.Arguments.Add(fileDefs);

            cmd.SetAction(parseResult =>
            {
                FileInfo? parsedFile = parseResult.GetValue(path);
                if (parsedFile == null || !parsedFile.Exists)
                {
                    Console.WriteLine("Input file does not exist.");
                    Console.WriteLine(parsedFile?.FullName);
                    return 1;
                }

                using var fs = File.OpenRead(parsedFile.FullName);

                // Peek the first 4 bytes to get endianess.
                byte[] versionBytes = new byte[4];
                fs.Read(versionBytes, 0, 4);

                // Detect Endianness
                var endian = Binary.DetectEndianness(versionBytes);
                Console.WriteLine($"Using '{endian}' endian.");

                var version = BitConverter.ToInt32(endian == Binary.Endianness.Small ? versionBytes : Binary.Reverse(ref versionBytes), 0);
                Console.WriteLine($"VList Version '{version}'.");

                // Rewind back to start
                fs.Seek(0, SeekOrigin.Begin);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nReading VList Data...\n");
                Console.ResetColor();

                var reader = new EndianBinaryReader(fs, endian);

                switch (version)
                {
                    case 6:
                        // VList v6 is the same as B3 Vehicle List
                        var vlistParserBo3 = new B3VehicleListParser();
                        var vlistFileBo3 = vlistParserBo3.Parse(reader);
                        Console.WriteLine(vlistFileBo3.ToString());
                        break;
                    case 9:
                        var vlistParserBo4 = new B4VehicleListParser();
                        var vlistFileBo4 = vlistParserBo4.Parse(reader);
                        Console.WriteLine(vlistFileBo4.ToString());
                        break;
                    default:
                        Console.WriteLine($"No Parser for Version '{version}'.");
                        break;
                }

                return 0;
            });

            return cmd;
        }
    }
}
