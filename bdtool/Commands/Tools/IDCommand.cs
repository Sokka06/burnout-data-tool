using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bdtool.Utilities;

namespace bdtool.Commands.Tools
{

    public static class IDCommand
    {
        public static Command Build()
        {
            var cmd = new Command("id", "GtID related tools");
            var compressCmd = new Command("compress", "Compresses a string");
            var uncompressCmd = new Command("uncompress", "Uncompresses an ulong to string");

            var inputArg = new Argument<string>("input") {
                Description = "Input value"
            };

            var verboseOpt = new Option<bool>("--verbose") { DefaultValueFactory = _ => false };

            compressCmd.Arguments.Add(inputArg);
            uncompressCmd.Arguments.Add(inputArg);
            compressCmd.Options.Add(verboseOpt);
            uncompressCmd.Options.Add(verboseOpt);

            cmd.Add(uncompressCmd);
            cmd.Add(compressCmd);

            uncompressCmd.SetAction(parseResult =>
            {
                var parsedLong = parseResult.GetValue(inputArg);
                if (string.IsNullOrEmpty(parsedLong))
                {
                    ConsoleEx.Error("No ulong value given.");
                    return 1;
                }

                var parsedVerbose = parseResult.GetValue(verboseOpt);

                if (parsedVerbose)
                {
                    ConsoleEx.Info($"\nCompressing ulong '{parsedLong}'");
                }

                var uncompressedText = GtID.Uncompress(ulong.Parse(parsedLong));
                ConsoleEx.Info(uncompressedText);
                return 0;
            });

            compressCmd.SetAction(parseResult =>
            {
                var parsedText = parseResult.GetValue(inputArg);
                if (string.IsNullOrEmpty(parsedText))
                {
                    ConsoleEx.Error("No text given.");
                    return 1;
                }

                var parsedVerbose = parseResult.GetValue(verboseOpt);

                if (parsedVerbose)
                {
                    ConsoleEx.Info($"\nCompressing Text '{parsedText}'");
                }

                var compressedText = GtID.Compress(parsedText);
                Console.WriteLine(compressedText);
                return 0;
            });

            return cmd;
        }
    }
}
