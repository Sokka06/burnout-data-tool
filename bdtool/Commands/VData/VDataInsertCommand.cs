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
            var cmd = new Command("insert", "Writes VDB Data into BGV/BTV files. Only works for Burnout Revenge vehicles.");
            
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
                    ConsoleEx.Error($"Target file does not exist at '{parsedTarget?.FullName}'.");
                    return 1;
                }
                
                var parsedData = parseResult.GetValue(dataArg);
                if (parsedData == null || !parsedData.Exists)
                {
                    ConsoleEx.Error($"Data file does not exist at '{parsedData?.FullName}'.");
                    return 1;
                }
                
                ConsoleEx.Break();
                ConsoleEx.Info($"Reading Vehicle Data...");

                using var fsTarget = File.Open(parsedTarget.FullName, FileMode.Open, FileAccess.ReadWrite);

                // Peek the first 4 bytes to get endian.
                var headerBytes = new byte[4];
                fsTarget.Read(headerBytes, 0, 4);
                
                // Detect endian
                var endian = Utilities.Binary.DetectEndian(headerBytes);
                ConsoleEx.Info($"Using '{endian}' endian.");

                var version = BitConverterE.ToInt32(headerBytes, endian);
                ConsoleEx.Info($"VData Version '{version}'.");

                // Versions before Burnout Revenge don't have a VDB section.
                if (version < 29)
                {
                    ConsoleEx.Error($"Version doesn't have a VDB section.");
                    return 1;
                }
                
                // Xbox 360 version uses a different offset.
                var startOffset = version switch
                {
                    37 => 0x2204,
                    _ => 0x2174
                };
                
                var vdbOffset = new byte[4];
                var vdbLength = new byte[4];
                
                fsTarget.Seek(startOffset, SeekOrigin.Begin);
                fsTarget.Read(vdbOffset, 0, 4);
                fsTarget.Read(vdbLength, 0, 4);

                var length = BitConverterE.ToInt32(vdbLength, endian);
                var offset = BitConverterE.ToInt32(vdbOffset, endian);
                
                using var fsData = File.OpenRead(parsedData.FullName);

                // Check VDB size difference.
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
                if (endian == Utilities.Binary.Endian.Big)
                    Utilities.Binary.Reverse(ref newLengthBytes);
                
                fsTarget.Seek(startOffset + 0x4, SeekOrigin.Begin);
                fsTarget.Write(newLengthBytes, 0, newLengthBytes.Length);
                
                ConsoleEx.Info($"VDB data written to '{parsedTarget.FullName}'\n");
                
                return 0;
            });

            return cmd;
        }
    }
}
