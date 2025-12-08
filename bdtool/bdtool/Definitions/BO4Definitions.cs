using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bdtool.Models.Common;

namespace bdtool.Definitions
{
    public static class BO4Definitions
    {
        public static readonly Dictionary<int, VariableDefinition> Schema = new Dictionary<int, VariableDefinition>
        {
            { 0, new VariableDefinition("", "", "", DataType.None, 0, 0f, 0f) }
        };
    }
}
