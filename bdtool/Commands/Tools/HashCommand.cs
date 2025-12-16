using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bdtool.Utilities;
using static bdtool.Utilities.Binary;

namespace bdtool.Commands.Tools
{
    public static class HashCommand
    {
        public static Command Build()
        {
            var cmd = new Command("hash", "Calculates Hash for string");

            //var input = new Option<string>("--input") { Required = true, Description = "Text to hash" };
            var endian = new Option<string>("--endian") { Description = "Endian (small or big)", DefaultValueFactory = ParseResult => "big" };
            var verbose = new Option<bool>("--verbose") { DefaultValueFactory = ParseResult => false } ;

            var value = new Argument<string>("value")
            {
                Description = "Text to hash",
            };

            endian.AcceptOnlyFromAmong("small", "big");

            cmd.Arguments.Add(value);
            //cmd.Options.Add(input);
            cmd.Options.Add(endian);
            cmd.Options.Add(verbose);

            cmd.SetAction(parseResult =>
            {
                string? parsedText = parseResult.GetValue(value);
                if (string.IsNullOrEmpty(parsedText))
                {
                    Console.WriteLine("No text given.");
                    return 1;
                }

                var parsedEndianess = parseResult.GetValue(endian) == "small" ? Endianness.Small : Endianness.Big;
                var parsedVerbose = parseResult.GetValue(verbose);

                var hashedText = Utilities.Hash.CalculateHash(parsedText);

                if (parsedVerbose)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"\nHashing Text '{parsedText}'");
                    Console.ResetColor();
                }

                Console.WriteLine($"0x{hashedText:X8} ({hashedText})");

                //ReadFile(parsedFile);
                return 0;
            });

            return cmd;
        }
    }
}
