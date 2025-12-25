using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bdtool.Binary;
using bdtool.Parsers;
using bdtool.Parsers.VList;
using bdtool.Utilities;

namespace bdtool.Commands.VList
{
    public static class VListReadCommand
    {
        public static Command Build()
        {
            var cmd = new Command("read", "Prints out VList file data.");
            
            var pathArg = new Argument<FileInfo>("path")
            {
                Description = "Path to the VList file."
            };

            /*var headerArg = new Argument<bool>("header")
            {
                Description = "Print header.",
                DefaultValueFactory = parseResult => true
            };

            var defaultValuesArg = new Argument<bool>("default")
            {
                Description = "Print default values.",
                DefaultValueFactory = parseResult => true
            };

            var valuesArg = new Argument<bool>("values")
            {
                Description = "Print values.",
                DefaultValueFactory = parseResult => true
            };

            var fileDefsArg = new Argument<bool>("defs")
            {
                Description = "Print file defs.",
                DefaultValueFactory = parseResult => true
            };*/
            
            var verboseOpt = new Option<bool>("--verbose", "-v");

            cmd.Arguments.Add(pathArg);
            /*cmd.Arguments.Add(headerArg);
            cmd.Arguments.Add(defaultValuesArg);
            cmd.Arguments.Add(valuesArg);
            cmd.Arguments.Add(fileDefsArg);*/
            
            cmd.Options.Add(verboseOpt);
            
            cmd.SetAction(parseResult =>
            {
                var parsedFile = parseResult.GetValue(pathArg);
                if (parsedFile == null || !parsedFile.Exists)
                {
                    Console.WriteLine("Input file does not exist.");
                    Console.WriteLine(parsedFile?.FullName);
                    return 1;
                }

                using var fs = File.OpenRead(parsedFile.FullName);

                // Peek the first 4 bytes to get endian.
                var versionBytes = new byte[4];
                fs.Read(versionBytes, 0, 4);

                // Detect endian
                var endian = Utilities.Binary.DetectEndian(versionBytes);
                ConsoleEx.Info($"Using '{endian}' endian.");

                var version = BitConverter.ToInt32(endian == Utilities.Binary.Endian.Little ? versionBytes : Utilities.Binary.Reverse(ref versionBytes), 0);
                ConsoleEx.Info($"VList Version '{version}'.");

                // Rewind back to start
                fs.Seek(0, SeekOrigin.Begin);

                ConsoleEx.Info("\nReading VList Data...\n");
                var reader = new BinaryReaderE(fs, endian);

                var parser = new VListParser();
                var vlist = parser.Read(reader);
                Console.WriteLine(vlist.ToString());
                
                return 0;
            });

            return cmd;
        }
    }
}
