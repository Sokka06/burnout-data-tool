using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using bdtool.Utilities;

namespace bdtool.Commands.VDB
{
    public static class WriteCommand
    {
        const string DEFAULT_PATH = "Data/Export/ValueDB/Physics.cfg";

        public static Command Build()
        {
            var cmd = new Command("write", "Writes VDB to a XML file.");

            var input = new Option<FileInfo>("--input") { Required = true, Description = "Path to the VDB file" };
            var output = new Option<string>("--output") { Description = "Path to the output XML file", DefaultValueFactory = parseResult => DEFAULT_PATH };
            var verbose = new Option<bool>("--verbose");

            cmd.Options.Add(input);
            cmd.Options.Add(output);
            cmd.Options.Add(verbose);

            cmd.SetAction(parseResult =>
            {
                FileInfo? parsedFile = parseResult.GetValue(input);
                if (parsedFile == null || !parsedFile.Exists)
                {
                    Console.WriteLine("Input file does not exist.");
                    return 1;
                }

                string? parsedPath = parseResult.GetValue(output);
                if (string.IsNullOrEmpty(parsedPath) || Path.GetDirectoryName(parsedPath) == string.Empty)
                {
                    Console.WriteLine($"Output path invalid: '{parsedPath}'");
                    return 1;
                }

                using var fs = File.OpenRead(parsedFile.FullName);

                // Peek the first 4 bytes to get endianess.
                byte[] headerBytes = new byte[4];
                fs.Read(headerBytes, 0, 4);

                // Detect Endianness
                var endian = Binary.DetectEndianness(headerBytes);
                Console.WriteLine($"Using '{endian}' endian.");

                // Rewind back to start
                fs.Seek(0, SeekOrigin.Begin); 
                var reader = new EndianBinaryReader(fs, endian);

                var vdbParser = new Parsers.VDBParser();
                var vdbFile = vdbParser.Parse(reader);
                Console.WriteLine($"File Header: {vdbFile.Header}");

                using (var writer = new XMLWriter(parsedPath))
                {
                    writer.Create("Physics");
                    writer.Finish();
                    Console.WriteLine($"Wrote data to '{Path.GetFullPath(parsedPath)}'");
                }

                //ReadFile(parsedFile);
                return 0;
            });

            return cmd;
        }

        public static void SaveVDBFileAsXML(string filePath)
        {
            // Filter entries matching the file path
            /*var entries = new List<CGtCommsDatabaseEntry>();
            for (int i = 0; i < 5000; i++)
            {
                if (mpElements[i].meDataType != 0 &&
                    string.Equals(maElements[i].mpcFileName, filePath,
                                 StringComparison.OrdinalIgnoreCase))
                {
                    entries.Add(mpElements[i]);
                }
            }

            // Sort by path
            entries.Sort((a, b) => string.Compare(a.mpcPath, b.mpcPath,
                                                  StringComparison.Ordinal));

            using (var writer = new XMLWriter(filePath))
            {
                string currentPath = "";
                int currentDepth = 0;

                foreach (var entry in entries)
                {
                    // Handle path hierarchy changes
                    if (currentPath != entry.mpcPath)
                    {
                        int newDepth = GetNumPathElements(entry.mpcPath);

                        // Close sections we're leaving
                        while (currentDepth > 0 &&
                               !PathsSharePrefix(currentPath, entry.mpcPath, currentDepth))
                        {
                            currentDepth--;
                            writer.CloseSection();
                        }

                        // Open new sections
                        while (currentDepth < newDepth)
                        {
                            string pathElement = GetPathElement(entry.mpcPath, currentDepth);
                            writer.OpenSection(pathElement);
                            currentDepth++;
                        }

                        currentPath = entry.mpcPath;
                    }

                    // Write the value based on type
                    switch (entry.meDataType)
                    {
                        case 1: // Integer
                            writer.WriteValue(entry.mpcValueName, *(int*)entry.mpData);
                            break;
                        case 2: // Float
                            writer.WriteValue(entry.mpcValueName, *(float*)entry.mpData);
                            break;
                        case 5: // Vector3
                            writer.WriteValue(entry.mpcValueName, *(Vector3*)entry.mpData);
                            break;
                    }
                }

                // Close all remaining sections
                while (currentDepth > 0)
                {
                    writer.CloseSection();
                    currentDepth--;
                }
            }*/
        }
    }
}
