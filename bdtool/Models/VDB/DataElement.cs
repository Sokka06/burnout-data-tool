using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace bdtool.Models.VDB
{
    public record DataElement
    {
        public int RawValue { get; init; }

        public DataElement() : this(default(int))
        {
        }

        public DataElement(int intValue)
        {
            RawValue = intValue;
        }

        public DataElement (float floatValue)
        {
            RawValue = BitConverter.SingleToInt32Bits(floatValue);
        }

        public DataElement(long longValue)
        {
            RawValue = (int)longValue;
        }

        public DataElement(bool boolValue)
        {
            RawValue = boolValue ? 1 : 0;
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
            return string.Format("{0,-20} {1,-20} {2,-20}", $"int32 ({AsInt()})", $"float ({AsFloat():F2})", $"bool ({AsBool()})");
            //return $"int32 ({AsInt()}), float ({AsFloat()}), bool ({AsBool()})";
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
