using System;
using System.Buffers.Binary;
using System.CommandLine;
using System.CommandLine.Parsing;
using System.IO;
using bdtool.Commands;
using bdtool.Commands.Tools;
using bdtool.Commands.VData;
using bdtool.Commands.VDB;
using bdtool.Commands.VList;

namespace bdtool
{
    class Program
    {
        static void Main(string[] args)
        {
            var root = new RootCommand("Burnout Data Tools");

            var vdb = new Command("vdb")
        {
            VDBReadCommand.Build(),
            VDBExportCommand.Build(),
            VDBImportCommand.Build()
        };

            var vlist = new Command("vlist")
        {
            VListReadCommand.Build(),
            VListExportCommand.Build(),
            VListImportCommand.Build()
        };

            var vdata = new Command("vdata")
            {
                VDataReadCommand.Build(),
            };

            var tools = new Command("tools")
        {
            HashCommand.Build(),
            HashNameCommand.Build(),
            IDCommand.Build(),
            CompareCommand.Build(),
            ReverseCommand.Build()
        };

            root.Subcommands.Add(vdb);
            root.Subcommands.Add(vlist);
            root.Subcommands.Add(vdata);
            root.Subcommands.Add(tools);
            root.Subcommands.Add(InfoCommand.Build());

            var result = root.Parse(args);

            /*
            foreach (ParseError parseError in result.Errors)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(parseError.Message);
                Console.ResetColor();
            }*/

            result.Invoke();
        }
    }
}