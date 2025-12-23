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

            var verboseOpt = new Option<bool>("--verbose", "-v");

            var pathArg = new Argument<FileInfo>("path")
            {
                Description = "Path to the BGV/BTV file."
            };

            cmd.Arguments.Add(pathArg);
            cmd.Options.Add(verboseOpt);

            cmd.SetAction(parseResult =>
            {
                var parsedFile = parseResult.GetValue(pathArg);
                if (parsedFile == null || !parsedFile.Exists)
                {
                    Console.WriteLine($"Input file does not exist at '{parsedFile?.FullName}'.");
                    return 1;
                }

                using var fs = File.OpenRead(parsedFile.FullName);

                // Peek the first 4 bytes to get endianess.
                var headerBytes = new byte[4];
                fs.Read(headerBytes, 0, 4);

                // Detect endian
                var endian = Utilities.Binary.DetectEndian(headerBytes);
                ConsoleEx.Info($"Using '{endian}' endian.");

                // Rewind back to start
                fs.Seek(0, SeekOrigin.Begin);

                ConsoleEx.Info($"Reading Vehicle Data...");
                ConsoleEx.Break();

                var reader = new BinaryReaderE(fs, endian);

                var vdataParser = new VehicleDataParser();
                var vdata = vdataParser.Read(reader);

                ConsoleEx.Break();
                Console.WriteLine(vdata.ToString());
                return 0;
            });

            return cmd;
        }
    }
}
