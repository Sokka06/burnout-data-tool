using System;
using System.Collections;
using System.Collections.Generic;
using System.CommandLine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bdtool.Utilities;

namespace bdtool.Commands.Tools
{

    public static class ReverseCommand
    {
        public static Command Build()
        {
            var cmd = new Command("reverse", "Reverses a 4 byte hex value. Used to convert a little endian value to big endian and vice versa.");

            var valueArg = new Argument<string>("value")
            {
                Description = "Hex value to be reversed",
            };

            var verboseOpt = new Option<bool>("--verbose") { DefaultValueFactory = ParseResult => false };

            cmd.Arguments.Add(valueArg);
            cmd.Options.Add(verboseOpt);

            cmd.SetAction(parseResult =>
            {
                var parsedText = parseResult.GetValue(valueArg);
                if (string.IsNullOrEmpty(parsedText))
                {
                    ConsoleEx.Error("No Hex value given.");
                    return 1;
                }
                
                // remove spaces between bytes
                parsedText = parsedText.Replace(" ", "");

                // remove prefix
                parsedText = (parsedText.StartsWith("0x", StringComparison.OrdinalIgnoreCase) ? parsedText[2..] : parsedText).Trim();

                var parsedVerbose = parseResult.GetValue(verboseOpt);

                if (parsedVerbose)
                {
                    ConsoleEx.Info($"\nReversing Hex value '{parsedText}'");
                }

                // reverse bytes.
                var bytes = Utilities.Binary.HexToBytes(parsedText);
                Utilities.Binary.Reverse(ref bytes);

                parsedText = Utilities.Binary.BytesToHex(bytes);

                ConsoleEx.Info($"{parsedText}");

                return 0;
            });

            return cmd;
        }
    }
}
