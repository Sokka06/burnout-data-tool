using System;
using System.Collections.Generic;
using System.CommandLine;
using System.IO;
using System.IO.Compression;
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
            var cmd = new Command("extract", "Extracts VDB Data from BGV/BTV files. Only works for Burnout Revenge vehicles.");
            
            var verboseOpt = new Option<bool>("--verbose", "-v");

            var pathArg = new Argument<FileInfo>("path")
            {
                Description = "Path to the BGV/BTV file."
            };
            
            var outPathArg = new Argument<string>("out")
            {
                Description = "Path to the output file."
            };

            cmd.Arguments.Add(pathArg);
            cmd.Arguments.Add(outPathArg);
            cmd.Options.Add(verboseOpt);

            cmd.SetAction(parseResult =>
            {
                var parsedFile = parseResult.GetValue(pathArg);
                if (parsedFile == null || !parsedFile.Exists)
                {
                    ConsoleEx.Error($"Input file does not exist at '{parsedFile?.FullName}'.");
                    return 1;
                }
                
                var parsedOut = parseResult.GetValue(outPathArg);
                if (string.IsNullOrEmpty(parsedOut) || Path.GetDirectoryName(parsedOut) == string.Empty)
                {
                    ConsoleEx.Error($"Output path invalid: '{parsedOut}'");
                    return 1;
                }

                if (!Path.HasExtension(parsedOut))
                {
                    ConsoleEx.Error($"No extension set for out path: '{parsedOut}'");
                    return 1;
                }

                using var fs = File.OpenRead(parsedFile.FullName);
                
                // Peek the first 4 bytes to get endian.
                var headerBytes = new byte[4];
                fs.Read(headerBytes, 0, 4);
                
                // Detect endian
                var endian = Utilities.Binary.DetectEndian(headerBytes);
                ConsoleEx.Info($"Using '{endian}' endian.");

                var version = BitConverterE.ToInt32(headerBytes, endian);
                ConsoleEx.Info($"VData Version '{version}'.");

                if (version < 29)
                {
                    ConsoleEx.Error($"Version doesn't have a VDB section.");
                    return 1;
                }
                
                ConsoleEx.Info($"\nReading Vehicle Data...\n");
                
                var vdbOffset = new byte[4];
                var vdbLength = new byte[4];

                var startOffset = version switch
                {
                    37 => 0x2204, // Xbox 360 version uses different offset.
                    _ => 0x2174 // 
                };

                fs.Seek(startOffset, SeekOrigin.Begin);
                fs.Read(vdbOffset, 0, 4);
                fs.Read(vdbLength, 0, 4);
                
                var length = BitConverterE.ToInt32(vdbLength, endian);
                var offset = BitConverterE.ToInt32(vdbOffset, endian);
                
                var vdbBytes = new byte[length];
                fs.Seek(offset, SeekOrigin.Begin);
                fs.Read(vdbBytes, 0, length);

                //var vdbBytes = parsedCompressed ? ReadCompressed(fs) : ReadUncompressed(fs);

                if (vdbBytes.Length == 0)
                {
                    ConsoleEx.Error($"Couldn't read file '{parsedFile.FullName}'.");
                    return 1;
                }

                ConsoleEx.Info($"Read VDB from file with length {vdbBytes.Length}.");
                
                ConsoleEx.Info("Writing VDB data to file...");
                File.WriteAllBytes(parsedOut, vdbBytes);
                
                ConsoleEx.Info($"VDB data saved to '{Path.GetFullPath(parsedOut)}'\n");
                
                return 0;
            });

            return cmd;
        }

        private static byte[] ReadUncompressed(FileStream fs)
        {
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
                return [];
            }

            // Rewind back to start
            //fs.Seek(0x2174, SeekOrigin.Begin);
                
            ConsoleEx.Info($"\nReading Vehicle Data...\n");
                
            var vdbOffset = new byte[4];
            var vdbLength = new byte[4];
                
            fs.Seek(8564, SeekOrigin.Begin);
            fs.Read(vdbOffset, 0, 4);
            fs.Read(vdbLength, 0, 4);

            var offset = BitConverter.ToInt32(vdbOffset, 0);
            var length = BitConverter.ToInt32(vdbLength, 0);
                
            var vdbBytes = new byte[length];
            fs.Seek(offset, SeekOrigin.Begin);
            fs.Read(vdbBytes, 0, length);

            return vdbBytes;
        }
        
        private static byte[] ReadCompressed(FileStream fs)
        {
            // Uncompress file.
            var zlibStream = new ZLibStream(fs, CompressionMode.Decompress);
            var buffer = new MemoryStream();
            zlibStream.CopyTo(buffer);
            buffer.Position = 0;
            
            // Peek the first 4 bytes to get endian.
            var headerBytes = new byte[4];
            buffer.Read(headerBytes, 0, 4);
                
            // X360 uses big endian.
            var endian = Utilities.Binary.Endian.Big;
            ConsoleEx.Info($"Using '{endian}' endian.");

            var version = BitConverter.ToInt32(Utilities.Binary.Reverse(ref headerBytes), 0);
            ConsoleEx.Info($"VData Version '{version}'.");

            if (version < 29)
            {
                ConsoleEx.Error($"Version doesn't have a VDB section.");
                return [];
            }
                
            ConsoleEx.Info($"\nReading Vehicle Data...\n");
                
            var vdbOffset = new byte[4];
            var vdbLength = new byte[4];
                
            buffer.Seek(0x2204, SeekOrigin.Begin);
            buffer.Read(vdbOffset, 0, 4);
            buffer.Read(vdbLength, 0, 4);

            var offset = BitConverter.ToInt32(Utilities.Binary.Reverse(ref vdbOffset), 0);
            var length = BitConverter.ToInt32(Utilities.Binary.Reverse(ref vdbLength), 0);
                
            var vdbBytes = new byte[length];
            buffer.Seek(offset, SeekOrigin.Begin);
            buffer.Read(vdbBytes, 0, length);

            return vdbBytes;
        }
    }
}
