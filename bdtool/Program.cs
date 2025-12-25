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
    public class Program
    {
        public static int Main(string[] args)
        {
            return BuildRootCommand().Parse(args).Invoke();
        }

        public static RootCommand BuildRootCommand()
        {
            var root = new RootCommand("Burnout Data Tool by Sokka06.\nGithub repo: https://github.com/Sokka06/burnout-data-tool");

            var vdb = new Command("vdb", "VDB commands. Used for VDB files found in 'data' folder.")
            {
                VDBReadCommand.Build(),
                VDBExportCommand.Build(),
                VDBImportCommand.Build()
            };

            var vlist = new Command("vlist", "Vehicle List commands. Used for VList files found in 'pveh' folder.")
            {
                VListReadCommand.Build(),
                VListExportCommand.Build(),
                VListImportCommand.Build()
            };

            var vdata = new Command("vdata", "Vehicle Data commands. Used for BGV/BTV files found in 'pveh' folders.")
            {
                VDataReadCommand.Build(),
                VDataExtractCommand.Build(),
                VDataInsertCommand.Build()
            };

            var tools = new Command("tools", "Useful tools.")
            {
                HashCommand.Build(),
                HashNameCommand.Build(),
                IDCommand.Build(),
                CompareCommand.Build(),
                ReverseCommand.Build(),
                ZLibCommand.Build(),
            };

            root.Subcommands.Add(vdb);
            root.Subcommands.Add(vlist);
            root.Subcommands.Add(vdata);
            root.Subcommands.Add(tools);
            root.Subcommands.Add(InfoCommand.Build());

            //var result = root.Parse(args);

            /*
            foreach (ParseError parseError in result.Errors)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(parseError.Message);
                Console.ResetColor();
            }*/

            return root;
        }

        /*static void Main(string[] args)
        {
            
        }*/
    }
}