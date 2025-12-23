namespace bdtool.Binary;

public static class BitConverterE
{
    public static int ToInt32(byte[] bytes, Utilities.Binary.Endian endian)
    {
        return BitConverter.ToInt32(endian == Utilities.Binary.Endian.Little ? bytes : Utilities.Binary.Reverse(ref bytes), 0);
    }
}