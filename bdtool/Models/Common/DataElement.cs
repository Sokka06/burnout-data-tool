using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace bdtool.Models.Common
{
    public record DataElement(int RawValue)
    {
        public DataElement() : this(default(int))
        {
        }

        public int AsInt()
        {
            return RawValue;
        }

        public float AsFloat()
        {
            return BitConverter.Int32BitsToSingle(RawValue);
        }

        public bool AsBool()
        {
            return RawValue != 0;
        }

        public override string ToString()
        {
            return $"int ({AsInt()}), float ({AsFloat()}), bool ({AsBool()})";
        }
    }

    /*[StructLayout(LayoutKind.Explicit, Size = 4)]
    public struct DataElement
    {
        [FieldOffset(0)] public int mnInt;
        [FieldOffset(0)] public float mrReal;
        [FieldOffset(0)] public int mbBool;  // RwBool as int32
        [FieldOffset(0)] public int mpnInt;  // Pointer as offset/index
        [FieldOffset(0)] public int mprReal;
        [FieldOffset(0)] public int mpbBool;
        [FieldOffset(0)] public int mpcChar;
        [FieldOffset(0)] public int mpVoid;
        [FieldOffset(0)] public int mpV3d;
        [FieldOffset(0)] public int mpfCallBack;

        public DataElement(int rawValue, DataType type)
        {
            switch (type)
            {
                case DataType.RwInt32:
                    mnInt = rawValue;
                    break;
                case DataType.RwReal:
                    mrReal = BitConverter.Int32BitsToSingle(rawValue);
                    break;
                case DataType.RwBool:
                    mbBool = rawValue;
                    break;
                case DataType.CGtV3d:
                    mpV3d = rawValue;
                    break;
                default:
                    mnInt = rawValue; // Default to int for unknown types
                    break;
            }
        }
    }*/
}
