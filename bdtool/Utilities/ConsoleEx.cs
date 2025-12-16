using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bdtool.Utilities
{
    public static class ConsoleEx
    {
        public static void Write(ConsoleColor color, string format, params object[] arg)
        {
            var current = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write(format, arg);
            Console.ForegroundColor = current;
        }

        public static void WriteLine(ConsoleColor color, string format, params object[] arg) =>
            Write(color, $"{format}\n", arg);

        public static void WriteBold(string format, params object[] arg) =>
            Console.Write($"\x1b[1m{format}\x1b[0m", arg);

        public static void WriteBoldLine(string format, params object[] arg) =>
           WriteBold($"{format}\n", arg);

        public static void Info(string format, params object[] arg)
        => WriteLine(ConsoleColor.Green, format, arg);

        public static void Warning(string format, params object[] arg)
            => WriteLine(ConsoleColor.Yellow, format, arg);

        public static void Error(string format, params object[] arg)
            => WriteLine(ConsoleColor.Red, format, arg);

        public static void Break() => Console.WriteLine();
    }
}
