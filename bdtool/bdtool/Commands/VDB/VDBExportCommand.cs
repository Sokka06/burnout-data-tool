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
using bdtool.Yaml;

namespace bdtool.Commands.VDB
{
    public static class VDBExportCommand
    {
        public static Command Build()
        {
            var cmd = new Command("export", "Exports a VDB file to a YAML file.");

            //var input = new Option<FileInfo>("--input", "-i") { Required = true, Description = "Path to the VDB file" };
            var verbose = new Option<bool>("--verbose", "-v");

            var path = new Argument<FileInfo>("path")
            {
                Description = "Path to the VDB file."
            };

            var outPath = new Argument<string>("out")
            {
                Description = "Path to the output directory."
            };

            //cmd.Options.Add(input);

            cmd.Arguments.Add(path);
            cmd.Arguments.Add(outPath);

            cmd.Options.Add(verbose);

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
                Console.WriteLine("\nParsing VDB Data...\n");
                Console.ResetColor();

                // Parse VDB file
                var reader = new EndianBinaryReader(fs, endian);
                var vdbParser = new VDBParser();
                var vdbFile = vdbParser.Read(reader);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nSerializing VDB Data to YAML...\n");
                Console.ResetColor();

                // Serialize to YAML
                var serializer = new YamlSerializer();
                var yamlText = serializer.Serialize(vdbFile);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nWriting data to YAML file...\n");
                Console.ResetColor();

                // Write YAML to file
                File.WriteAllText(parsedOut, yamlText);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\nYAML file saved to '{Path.GetFullPath(parsedOut)}'\n");
                Console.ResetColor();

                return 0;
            });

            return cmd;
        }
    }
}
