using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bdtool.Models.Common;
using bdtool.Utilities;
using static bdtool.Utilities.Binary;

namespace bdtool.Definitions
{
    public record VariableDefinition
    (
        string Name,
        string Path,      // Category
        string FileName,  // Source file
        DataType Type,
        byte NumElements, // for arrays?
        float MinValue,
        float MaxValue
    )
    {
        // Compute hash using the game's algorithm
        public int ComputeHash() => Comms.HashName(Name, Path, FileName);

    }
}
