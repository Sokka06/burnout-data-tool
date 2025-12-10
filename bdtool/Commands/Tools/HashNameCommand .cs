using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static bdtool.Utilities.Binary;

namespace bdtool.Commands.Tools
{

    public static class HashNameCommand
    {
        public static Command Build()
        {
            var cmd = new Command("hashname", "Calculates Hash for a default variable using name, path and filename");

            //var name = new Option<string>("--name") { Required = true, Description = "Name" };
            //var path = new Option<string>("--path") { Required = true, Description = "Path" };
            //var fileName = new Option<string>("--filename") { Required = true, Description = "File name" };
            //var endianess = new Option<string>("--endianess") { Description = "Endianness (small or big)", DefaultValueFactory = ParseResult => "small" };
            var verbose = new Option<bool>("--verbose") { DefaultValueFactory = ParseResult => false } ;

            //endianess.AcceptOnlyFromAmong("small", "big");

            var name = new Argument<string>("name")
            {
                Description = "Name",
                DefaultValueFactory = parseResult => ""
            };

            var path = new Argument<string>("path")
            {
                Description = "Path",
                DefaultValueFactory = parseResult => ""
            };

            var fileName = new Argument<string>("filename")
            {
                Description = "File name",
                DefaultValueFactory = parseResult => ""
            };

            cmd.Arguments.Add(name);
            cmd.Arguments.Add(path);
            cmd.Arguments.Add(fileName);

            //cmd.Options.Add(name);
            //cmd.Options.Add(path);
            //cmd.Options.Add(fileName);
            //cmd.Options.Add(endianess);
            cmd.Options.Add(verbose);

            cmd.SetAction(parseResult =>
            {
                string? parsedName = parseResult.GetValue(name);
                /*if (string.IsNullOrEmpty(parsedName))
                {
                    Console.WriteLine("Missing 'Name' argument.");
                    return 1;
                }*/

                string? parsedPath = parseResult.GetValue(path);
                /*if (string.IsNullOrEmpty(parsedPath))
                {
                    Console.WriteLine("Missing 'Path' argument.");
                    return 1;
                }*/

                string? parsedFileName = parseResult.GetValue(fileName);
                /*if (string.IsNullOrEmpty(parsedFileName))
                {
                    Console.WriteLine("Missing 'File Name' argument.");
                    return 1;
                }*/

                bool parsedVerbose = parseResult.GetValue(verbose);

                var hash = Utilities.Comms.HashName(parsedName, parsedPath, parsedFileName);

                if (parsedVerbose)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"\nHashing: '{parsedName}' + '{parsedPath}' + '{parsedFileName}'");
                    Console.ResetColor();
                }

                Console.WriteLine(hash);

                //ReadFile(parsedFile);
                return 0;
            });

            return cmd;
        }
    }
}
