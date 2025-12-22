using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bdtool.Models.VDB
{

    public record VDBFile
    {
        public VDBHeader Header { get; init; }
        public List<DatabaseDefaultValue> DefaultValues { get; init; }
        public List<DatabaseValue> Values { get; init; }
        public List<DatabaseFileDef> FileDefs { get; init; }

        /*public VDBFile() : this(default, default, default, default)
        {
        }*/

        public string PrintHeader()
        {
            var builder = new StringBuilder();

            builder.AppendLine("[Header]");
            builder.Append(Header.ToString());
            return builder.ToString();
        }

        public string PrintDefaultValues()
        {
            var builder = new StringBuilder();

            builder.AppendLine("[Default Values]");
            foreach (var val in DefaultValues)
            {
                builder.AppendLine(val.ToString());
            }

            return builder.ToString();
        }

        public string PrintValues()
        {
            var builder = new StringBuilder();

            builder.AppendLine("[Values]");
            foreach (var val in Values)
            {
                builder.AppendLine(val.ToString());
            }

            return builder.ToString();
        }

        public string PrintFileDefs()
        {
            var builder = new StringBuilder();

            builder.AppendLine("[File Defs]");
            foreach (var fileDef in FileDefs)
            {
                builder.AppendLine(fileDef.ToString());
            }

            return builder.ToString();
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.AppendLine(PrintHeader());
            builder.AppendLine(PrintDefaultValues());
            builder.AppendLine(PrintValues());
            builder.AppendLine(PrintFileDefs());

            return builder.ToString();
        }


    }
}
