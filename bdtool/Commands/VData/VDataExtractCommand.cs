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
    public static class VDataExtractCommand
    {
        public static Command Build()
        {
            var cmd = new Command("extract", "Extracts Vehicle Data from BGV/BTV files.");

            var verboseOpt = new Option<bool>("--verbose", "-v");

            var pathArg = new Argument<FileInfo>("path")
            {
                Description = "Path to the BGV/BTV file."
            };
            
            var outPathArg = new Argument<string>("out")
            {
                Description = "Path to the output directory."
            };

            cmd.Arguments.Add(pathArg);
            cmd.Arguments.Add(outPathArg);
            cmd.Options.Add(verboseOpt);

            cmd.SetAction(parseResult =>
            {
                var parsedFile = parseResult.GetValue(pathArg);
                if (parsedFile == null || !parsedFile.Exists)
                {
                    Console.WriteLine($"Input file does not exist at '{parsedFile?.FullName}'.");
                    return 1;
                }
                
                var parsedOut = parseResult.GetValue(outPathArg);
                if (string.IsNullOrEmpty(parsedOut) || Path.GetDirectoryName(parsedOut) == string.Empty)
                {
                    ConsoleEx.Error($"Output path invalid: '{parsedOut}'");
                    return 1;
                }

                using var fs = File.OpenRead(parsedFile.FullName);

                // Peek the first 4 bytes to get endian.
                var headerBytes = new byte[4];
                fs.Read(headerBytes, 0, 4);
                
                // Detect endian
                var endian = Utilities.Binary.DetectEndian(headerBytes);
                ConsoleEx.Info($"Using '{endian}' endian.");

                var version = BitConverter.ToInt32(endian == Utilities.Binary.Endian.Little ? headerBytes : Utilities.Binary.Reverse(ref headerBytes), 0);
                ConsoleEx.Info($"VData Version '{version}'.");

                if (version < 29)
                {
                    ConsoleEx.Error($"Version doesn't have a VDB section.");
                    return 1;
                }

                // Rewind back to start
                //fs.Seek(0x2174, SeekOrigin.Begin);
                
                ConsoleEx.Info($"\nReading {parsedFile.Extension.Substring(1).ToUpper()} Data...\n");
                
                var vdbOffset = new byte[4];
                var vdbLength = new byte[4];
                
                fs.Seek(8564, SeekOrigin.Begin);
                fs.Read(vdbOffset, 0, 4);
                fs.Read(vdbLength, 0, 4);

                var length = BitConverter.ToInt32(vdbLength, 0);
                var offset = BitConverter.ToInt32(vdbOffset, 0);
                
                var vdbBytes = new byte[length];
                fs.Seek(offset, SeekOrigin.Begin);
                fs.Read(vdbBytes, 0, length);

                ConsoleEx.Info($"Read VDB from file at {offset} with length {length}.");
                
                ConsoleEx.Info("Writing VDB data to file...");
                File.WriteAllBytes(parsedOut, vdbBytes);
                
                ConsoleEx.Info($"VDB data saved to '{Path.GetFullPath(parsedOut)}'\n");
                
                return 0;
            });

            return cmd;
        }
    }
}
