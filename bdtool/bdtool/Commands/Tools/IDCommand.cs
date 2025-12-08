using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bdtool.Commands.Tools
{

    public static class IDCommand
    {
        public static Command Build()
        {
            var cmd = new Command("id", "GtID related tools");
            var compressCmd = new Command("compress", "Compresses a string");
            var uncompressCmd = new Command("uncompress", "Uncompresses an ulong to string");

            var input = new Argument<string>("input") {
                Description = "Input value"
            };

            //var input = new Option<string>("--input") { Required = true, Description = "Input value" };
            var verbose = new Option<bool>("--verbose") { DefaultValueFactory = ParseResult => false };

            compressCmd.Arguments.Add(input);
            uncompressCmd.Arguments.Add(input);
            compressCmd.Options.Add(verbose);
            uncompressCmd.Options.Add(verbose);

            cmd.Add(uncompressCmd);
            cmd.Add(compressCmd);

            uncompressCmd.SetAction(parseResult =>
            {
                string? parsedLong = parseResult.GetValue(input);
                if (string.IsNullOrEmpty(parsedLong))
                {
                    Console.WriteLine("No ulong value given.");
                    return 1;
                }

                bool parsedVerbose = parseResult.GetValue(verbose);

                if (parsedVerbose)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"\nCompressing ulong '{parsedLong}'");
                    Console.ResetColor();
                }

                var uncompressedText = Utilities.GtID.GtIDUnCompress(ulong.Parse(parsedLong));
                Console.WriteLine(uncompressedText);
                return 0;
            });

            compressCmd.SetAction(parseResult =>
            {
                string? parsedText = parseResult.GetValue(input);
                if (string.IsNullOrEmpty(parsedText))
                {
                    Console.WriteLine("No text given.");
                    return 1;
                }

                bool parsedVerbose = parseResult.GetValue(verbose);

                if (parsedVerbose)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"\nCompressing Text '{parsedText}'");
                    Console.ResetColor();
                }

                var compressedText = Utilities.GtID.GtIDCompress(parsedText);
                Console.WriteLine(compressedText);
                return 0;
            });

            return cmd;
        }
    }
}
