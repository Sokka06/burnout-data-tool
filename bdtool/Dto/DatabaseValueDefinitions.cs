using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bdtool.Definitions;
using bdtool.Utilities;
using YamlDotNet.Serialization;

namespace bdtool.Dto
{
    // Types from the BO4
    /*public enum DataType : byte
    {
        None = 0,
        RwInt32 = 1,
        RwReal = 2,
        RwBool = 3,
        unk4 = 4, // unused?
        V3d = 5,
        Callback = 6
    }*/

    public enum DataType : byte
    {
        None = 0,
        RwInt32 = 1,
        RwReal = 2,
        RwBool = 3,
        V3d = 4,
        Pointer = 5
    }

    public record DatabaseValueDefinitions
    {
        public required Dictionary<int, DefaultValueEntry> DefaultValues { get; init; }
        public required Dictionary<long, ValueEntry> Values { get; init; }
        public required Dictionary<int, FilesEntry> Files { get; init; }

        public record DefaultValueEntry
        {
            public int Hash { get; init; }
            public required string Name { get; init; }
            public required string Path { get; init; }      // Category
            public required string FileName { get; init; }  // Source file
            public DataType Type { get; init; }
            //public byte NumElements { get; init; } // for arrays?
        }

        public record ValueEntry
        {
            public DataType Type { get; init; }
        }

        public record FilesEntry
        {
            public required string FileName { get; init; }
        }
    }
}
