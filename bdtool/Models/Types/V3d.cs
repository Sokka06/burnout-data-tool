using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bdtool.Models.Types
{
    public record V3d
    {
        public float X { get; init; }
        public float Y { get; init; }
        public float Z { get; init; }
        public float Padding { get; init; }

        public float this[int index]
        {
            get 
            {
                switch (index)
                {
                    case 0:
                        return X;
                    case 1:
                        return Y;
                    case 2:
                        return Z;
                    case 3:
                        return Padding;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
        }

        public override string ToString()
        {
            return $"{{X: {X:F2}, Y: {Y:F2}, Z: {Z:F2}}}";
        }
    }
}
