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
            //var endian = new Option<Endian>("--endian") { Description = "Endian (Little or Big)", DefaultValueFactory = ParseResult => Endian.Little };
            var verbose = new Option<bool>("--verbose") { DefaultValueFactory = ParseResult => false } ;

            var value = new Argument<string>("value")
            {
                Description = "Text to hash",
            };

            //endian.AcceptOnlyFromAmong("little", "big");

            cmd.Arguments.Add(value);
            //cmd.Options.Add(input);
            //cmd.Options.Add(endian);
            cmd.Options.Add(verbose);

            cmd.SetAction(parseResult =>
            {
                string? parsedText = parseResult.GetValue(value);
                if (string.IsNullOrEmpty(parsedText))
                {
                    ConsoleEx.Error("No text given.");
                    return 1;
                }

                //var parsedEndian = parseResult.GetValue(endian);
                var parsedVerbose = parseResult.GetValue(verbose);

                var hashValue = Hash.CalculateHash(parsedText);

                if (parsedVerbose)
                {
                    ConsoleEx.Info($"\nHashing Text '{parsedText}'");
                }

                ConsoleEx.Info($"0x{hashValue:X8} ({hashValue})");
                return 0;
            });

            return cmd;
        }
    }
}
