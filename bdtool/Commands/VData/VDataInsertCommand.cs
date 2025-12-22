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
    public static class VDataInsertCommand
    {
        public static Command Build()
        {
            var cmd = new Command("insert", "Overwrites VDB Data in BGV/BTV files.");

            var verboseOpt = new Option<bool>("--verbose", "-v");

            var targetArg = new Argument<FileInfo>("target")
            {
                Description = "Path to the target BGV/BTV file."
            };
            
            var dataArg = new Argument<FileInfo>("data")
            {
                Description = "Path to data file that will be inserted to the target file."
            };

            cmd.Arguments.Add(targetArg);
            cmd.Arguments.Add(dataArg);
            cmd.Options.Add(verboseOpt);

            cmd.SetAction(parseResult =>
            {
                var parsedTarget = parseResult.GetValue(targetArg);
                if (parsedTarget == null || !parsedTarget.Exists)
                {
                    Console.WriteLine($"Target file does not exist at '{parsedTarget?.FullName}'.");
                    return 1;
                }
                
                var parsedData = parseResult.GetValue(dataArg);
                if (parsedData == null || !parsedData.Exists)
                {
                    Console.WriteLine($"Data file does not exist at '{parsedData?.FullName}'.");
                    return 1;
                }
                
                ConsoleEx.Break();
                ConsoleEx.Info($"Reading {parsedTarget.Extension.Substring(1).ToUpper()} Data...");

                using var fsTarget = File.Open(parsedTarget.FullName, FileMode.Open, FileAccess.ReadWrite);

                // Peek the first 4 bytes to get endian.
                var headerBytes = new byte[4];
                fsTarget.Read(headerBytes, 0, 4);
                
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
                
                var vdbOffset = new byte[4];
                var vdbLength = new byte[4];
                
                fsTarget.Seek(8564, SeekOrigin.Begin);
                fsTarget.Read(vdbOffset, 0, 4);
                fsTarget.Read(vdbLength, 0, 4);

                var length = BitConverter.ToInt32(vdbLength, 0);
                var offset = BitConverter.ToInt32(vdbOffset, 0);
                
                using var fsData = File.OpenRead(parsedData.FullName);

                if (length != fsData.Length)
                {
                    ConsoleEx.Warning("Size does not match. \nPress Y to continue or ESC to abort...");
                    ConsoleKeyInfo cki;
                    do
                    {
                        cki = Console.ReadKey();
                        if (cki.Key != ConsoleKey.Escape) 
                            continue;
                    
                        ConsoleEx.Info("Aborting...");
                        return 1;
                    } while (cki.Key != ConsoleKey.Y);

                    ConsoleEx.Info("Continuing...");
                    return 0;
                }
                
                ConsoleEx.Info("Writing VDB data to target file...");
                
                var vdbBytes = new byte[fsData.Length];
                fsData.Read(vdbBytes, 0, vdbBytes.Length);
                
                fsTarget.Seek(offset, SeekOrigin.Begin);
                fsTarget.Write(vdbBytes,  0, vdbBytes.Length);
                
                // write new length
                var newLengthBytes = BitConverter.GetBytes(vdbBytes.Length);
                fsTarget.Seek(8568, SeekOrigin.Begin);
                fsTarget.Write(endian == Utilities.Binary.Endian.Little ? newLengthBytes : Utilities.Binary.Reverse(ref newLengthBytes),  0, newLengthBytes.Length);
                
                ConsoleEx.Info($"VDB data written to '{parsedTarget.FullName}'\n");
                
                // TODO: adjust VDB size in BGV header.
                
                return 0;
            });

            return cmd;
        }
    }
}
