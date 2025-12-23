using System.CommandLine;
using System.IO.Compression;
using bdtool.Utilities;

namespace bdtool.Commands.Tools
{
    public static class ZLibCommand
    {
        public static Command Build()
        {
            var cmd = new Command("zlib", "ZLib related tools");
            var compressCmd = new Command("compress", "Compresses a file");
            var uncompressCmd = new Command("uncompress", "Uncompresses a file");

            var inputArg = new Argument<FileInfo>("input")
            {
                Description = "Path to the input file."
            };
            
            var outputArg = new Argument<string>("output")
            {
                Description = "Path to the output file."
            };

            compressCmd.Arguments.Add(inputArg);
            compressCmd.Arguments.Add(outputArg);
            uncompressCmd.Arguments.Add(inputArg);
            uncompressCmd.Arguments.Add(outputArg);

            cmd.Add(uncompressCmd);
            cmd.Add(compressCmd);

            uncompressCmd.SetAction(parseResult =>
            {
                var parsedFile = parseResult.GetValue(inputArg);
                if (parsedFile == null || !parsedFile.Exists)
                {
                    ConsoleEx.Error($"Input file does not exist at '{parsedFile?.FullName}'.");
                    return 1;
                }

                var parsedOut = parseResult.GetValue(outputArg);
                if (string.IsNullOrEmpty(parsedOut) || Path.GetDirectoryName(parsedOut) == string.Empty)
                {
                    ConsoleEx.Error($"Output path invalid: '{parsedOut}'");
                    return 1;
                }
                
                using var fs = File.OpenRead(parsedFile.FullName);
                
                ConsoleEx.Break();
                ConsoleEx.Info("Uncompressing file...");
                
                // Uncompress file.
                var zlibStream = new ZLibStream(fs, CompressionMode.Decompress);
                using var output = new MemoryStream();
                zlibStream.CopyTo(output);
                var decompressed = output.ToArray();
                
                File.WriteAllBytes(parsedOut, decompressed);
                
                ConsoleEx.Info($"Uncompressed file saved to '{parsedOut}'.");
                ConsoleEx.Break();
                
                return 0;
            });

            compressCmd.SetAction(parseResult =>
            {
                var parsedFile = parseResult.GetValue(inputArg);
                if (parsedFile == null || !parsedFile.Exists)
                {
                    ConsoleEx.Error($"Input file does not exist at '{parsedFile?.FullName}'.");
                    return 1;
                }

                var parsedOut = parseResult.GetValue(outputArg);
                if (string.IsNullOrEmpty(parsedOut) || Path.GetDirectoryName(parsedOut) == string.Empty)
                {
                    ConsoleEx.Error($"Output path invalid: '{parsedOut}'");
                    return 1;
                }
                
                using var input = File.OpenRead(parsedFile.FullName);
                
                ConsoleEx.Break();
                ConsoleEx.Info("Compressing file...");
                
                // Uncompress file.
                // Create output file
                using var outputFile = File.Create(parsedOut);
                using var zlibStream = new ZLibStream(outputFile, CompressionLevel.SmallestSize);
                input.CopyTo(zlibStream);
                
                ConsoleEx.Info($"Compressed file saved to '{parsedOut}'.");
                ConsoleEx.Break();
                
                return 0;
            });

            return cmd;
        }
    }
}