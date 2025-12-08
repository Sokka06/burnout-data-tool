using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bdtool.Models.Common
{
    public enum DataType : byte
    {
        None = 0,
        RwInt32 = 1,
        RwReal = 2,
        RwBool = 3,
        unk4 = 4, // unused?
        CGtV3d = 5,
        Callback = 6
    }

    public record VDBFile
    (
        VDBHeader Header, 
        List<DatabaseDefaultValue> DefaultValues,
        List<DatabaseValue> Values,
        List<DatabaseFileDef> FileDefs
    )
    {
        public VDBFile() : this(default, default, default, default)
        {
        }

        public string PrintHeader()
        {
            var builder = new StringBuilder();

            builder.AppendLine("[Header]");
            builder.AppendLine(Header.ToString());
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

            /*builder.AppendLine("[Header]");
            builder.AppendLine(Header.ToString());

            builder.AppendLine("\n[Default Values]");
            foreach (var val in DefaultValues)
            {
                builder.AppendLine(val.ToString());
            }

            builder.AppendLine("\n[Values]");
            foreach (var val in Values)
            {
                builder.AppendLine(val.ToString());
            }

            builder.AppendLine("\n[File Defs]");
            foreach (var fileDef in FileDefs)
            {
                builder.AppendLine(fileDef.ToString());
            }*/

            return builder.ToString();
        }
    }
}
