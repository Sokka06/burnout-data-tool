using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bdtool.Commands
{
    public static class InfoCommand
    {
        public static Command Build()
        {
            var cmd = new Command("info", "Prints info about the different data files.");

            var section = new Argument<string>("section")
            {
                Description = "Selects a section to print info about."
            };

            section.AcceptOnlyFromAmong("vdb", "vlist");

            //cmd.Options.Add(section);
            cmd.Arguments.Add(section);

            cmd.SetAction(parseResult =>
            {
                string? parsedSection = parseResult.GetValue(section);
                if (string.IsNullOrEmpty(parsedSection))
                {
                    Console.WriteLine("No data type selected. Use \"vdb\", \"vlist\" values to select a type.");
                    return 1;
                }

                var infoText = "";

                switch (parsedSection)
                {
                    case "vdb":
                        infoText = "Add info about the VDB file here.";
                        break; 
                    case "vlist":
                        infoText = "Add info about the VList file here.";
                        break;
                    default:
                        break;
                }

                Console.WriteLine(infoText);
                return 0;
            });

            return cmd;
        }
    }
}
