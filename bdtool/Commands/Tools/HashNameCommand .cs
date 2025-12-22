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

    public static class HashNameCommand
    {
        public static Command Build()
        {
            var cmd = new Command("hashname", "Calculates Hash for a default variable using name, path and filename");
            var verboseOpt = new Option<bool>("--verbose") { DefaultValueFactory = ParseResult => false } ;

            var nameArg = new Argument<string>("name")
            {
                Description = "Name",
                DefaultValueFactory = parseResult => ""
            };

            var pathArg = new Argument<string>("path")
            {
                Description = "Path",
                DefaultValueFactory = parseResult => ""
            };

            var fileNameArg = new Argument<string>("filename")
            {
                Description = "File name",
                DefaultValueFactory = parseResult => ""
            };

            cmd.Arguments.Add(nameArg);
            cmd.Arguments.Add(pathArg);
            cmd.Arguments.Add(fileNameArg);

            cmd.Options.Add(verboseOpt);

            cmd.SetAction(parseResult =>
            {
                var name = parseResult.GetValue(nameArg);
                var path = parseResult.GetValue(pathArg);
                var fileName = parseResult.GetValue(fileNameArg);
                
                var parsedVerbose = parseResult.GetValue(verboseOpt);

                var hashValue = Hash.HashName(name, path, fileName);

                if (parsedVerbose)
                {
                    ConsoleEx.Info($"\nHashing: '{name}' + '{path}' + '{fileName}'");
                }
                
                ConsoleEx.Info($"0x{hashValue:X8} ({hashValue})");
                return 0;
            });

            return cmd;
        }
    }
}
