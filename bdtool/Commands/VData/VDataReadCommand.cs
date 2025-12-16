using System;
using System.Collections.Generic;
using System.CommandLine;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bdtool.Binary;
using bdtool.Parsers;
using bdtool.Parsers.VData;
using bdtool.Utilities;

namespace bdtool.Commands.VData
{
    public static class VDataReadCommand
    {
        public static Command Build()
        {
            var cmd = new Command("read", "Prints out Vehicle Data from BGV/BTV files.");

            //var input = new Option<FileInfo>("--input", "-i") { Required = true, Description = "Path to the VDB file" };
            var verbose = new Option<bool>("--verbose", "-v");

            var path = new Argument<FileInfo>("path")
            {
                Description = "Path to the BGV/BTV file."
            };

            //cmd.Options.Add(input);

            cmd.Arguments.Add(path);

            cmd.Options.Add(verbose);

            cmd.SetAction(parseResult =>
            {
                FileInfo? parsedFile = parseResult.GetValue(path);
                if (parsedFile == null || !parsedFile.Exists)
                {
                    Console.WriteLine($"Input file does not exist at '{parsedFile?.FullName}'.");
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
                Console.WriteLine($"\nReading {parsedFile.Extension.Substring(1).ToUpper()} Data...\n");
                Console.ResetColor();

                var reader = new BinaryReaderE(fs, endian);

                var vdataParser = new VehicleDataParser();
                var vdata = vdataParser.Read(reader);

                Console.WriteLine(vdata.ToString());

                return 0;
            });

            return cmd;
        }
    }
}
