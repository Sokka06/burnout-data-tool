using System;
using System.Collections;
using System.Collections.Generic;
using System.CommandLine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bdtool.Utilities;
using static bdtool.Utilities.Binary;

namespace bdtool.Commands.Tools
{

    public static class ReverseCommand
    {
        public static Command Build()
        {
            var cmd = new Command("reverse", "Reverses a hex value. Used to convert a small endian value to big endian and vice versa.");

            var value = new Argument<string>("value")
            {
                Description = "Hex value to be reversed",
            };

            var verbose = new Option<bool>("--verbose") { DefaultValueFactory = ParseResult => false };

            cmd.Arguments.Add(value);
            cmd.Options.Add(verbose);

            cmd.SetAction(parseResult =>
            {
                var parsedText = parseResult.GetValue(value);
                if (string.IsNullOrEmpty(parsedText))
                {
                    Console.WriteLine("No Hex value given.");
                    return 1;
                }

                // prepare hex text.
                parsedText = (parsedText.StartsWith("0x", StringComparison.OrdinalIgnoreCase) ? parsedText[2..] : parsedText).Trim();

                var parsedVerbose = parseResult.GetValue(verbose);

                if (parsedVerbose)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"\nReversing Hex value '{parsedText}'");
                    Console.ResetColor();
                }

                // reverse bytes.
                var bytes = Utilities.Binary.HexToBytes(parsedText);
                Utilities.Binary.Reverse(ref bytes);

                parsedText = Utilities.Binary.BytesToHex(bytes);

                Console.WriteLine($"{parsedText}");

                return 0;
            });

            return cmd;
        }
    }
}
