using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bdtool.Models.Types
{
    public record Matrix3x4
    {
        public required V3d Right { get; init; }
        public required V3d Up { get; init; }
        public required V3d At { get; init; }
        public required V3d Pos { get; init; }

        public override string ToString()
        {


            return $"{{Right: {Right}, Up: {Up}, At: {At}, Pos: {Pos}}}";
        }
    }
}
