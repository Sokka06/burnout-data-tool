using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bdtool.Commands.Tools
{
    public static class VerifyCommand
    {
        public static Command Build()
        {
            var cmd = new Command("verify", "Compare source file to target file byte by byte. Note that endianness is not converted, files that use a different endian will differ immediately.");

            var verbose = new Option<bool>("--verbose", "-v");

            var source = new Argument<FileInfo>("source")
            {
                Description = "Path to source file."
            };

            var target = new Argument<FileInfo>("target")
            {
                Description = "Path to target file."
            };

            cmd.Options.Add(verbose);

            cmd.Arguments.Add(source);
            cmd.Arguments.Add(target);

            cmd.SetAction(parseResult =>
            {
                FileInfo? parsedSource = parseResult.GetValue(source);
                if (parsedSource == null || !parsedSource.Exists)
                {
                    Console.WriteLine("Source file does not exist.");
                    return 1;
                }

                FileInfo? parsedTarget = parseResult.GetValue(target);
                if (parsedTarget == null || !parsedTarget.Exists)
                {
                    Console.WriteLine("Target file does not exist.");
                    return 1;
                }

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\nComparing '{parsedSource.Name}' to '{parsedTarget.Name}' byte to byte...");
                Console.ResetColor();

                var comparer = new Utilities.ReadWholeFileAtOnce(parsedSource, parsedTarget);
                var areSame = comparer.Compare(out var position);

                if (areSame)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"\nResult: Both files match perfectly!\n");
                    Console.ResetColor();
                }
                else
                {
                    // if 

                    // Get next 4 bytes at position
                    var sourceBytes = new byte[4];
                    var targetBytes = new byte[4];

                    for (int i = 0; i < 2; i++)
                    {
                        if (position == -1)
                            break;

                        var file = i == 0 ? parsedSource : parsedTarget;
                        var bytes = i == 0 ? sourceBytes : targetBytes;
                        using (BinaryReader reader = new(new FileStream(file.FullName, FileMode.Open)))
                        {
                            if (file.Length < position)
                                continue;

                            reader.BaseStream.Seek(position, SeekOrigin.Begin);
                            reader.Read(bytes, 0, 4);
                        }
                    }

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"\nResult: '{parsedSource.Name}' does NOT match '{parsedTarget.Name}' at position '{position} (0x{position:X})' out of '{parsedSource.Length}' total,");
                    Console.WriteLine($"[{BitConverter.ToString(sourceBytes).Replace("-", " ")}] - [{BitConverter.ToString(targetBytes).Replace("-", " ")}].\n");
                    Console.ResetColor();
                }

                return 0;
            });

            return cmd;
        }
    }
}
