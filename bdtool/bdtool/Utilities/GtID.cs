using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bdtool.Utilities
{
    public static class GtID
    {
        /// <summary>
        /// Converts ID back to a string.
        /// GtIDConvertToString__FUlPc from BO4 Prototype for PS2.
        /// </summary>
        /// <returns></returns>
        public static string GtIDConvertToString(ulong id)
        {
            string result = GtIDUnCompress(id);
            return result.TrimEnd(' ');
        }

        /// <summary>
        /// Converts a compressed ID back to a string.
        /// GtIDUnCompress__FUlPc from BO4 Prototype for PS2.
        /// </summary>
        public static string GtIDUnCompress(ulong id)
        {
            char[] buffer = new char[12];

            for (int i = 11; i >= 0; i--)
            {
                char cVar1 = (char)(id % 0x28);

                buffer[i] = cVar1 switch
                {
                    '\'' => '_',
                    '\x02' => '/',
                    '\x01' => '-',
                    '\0' => ' ',
                    >= '\r' => (char)(cVar1 + '4'),
                    >= '\x03' => (char)(cVar1 + '-'),
                    //_ => '\0'
                };

                id /= 0x28;
            }

            return new string(buffer).TrimEnd('\0');
        }

        /// <summary>
        /// Converts a string to a compressed ID.
        /// GtIDCompress__FPCc from BO4 Prototype for PS2.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static ulong GtIDCompress(string str)
        {
            ulong GVar3 = 0;
            int iVar4 = 0;

            if (!string.IsNullOrEmpty(str))
            {
                long lVar2 = 0;
                int index = 0;

                while (index < str.Length)
                {
                    char cVar1 = str[index];
                    GVar3 = (ulong)(lVar2 + (long)GVar3) * 8;

                    if (cVar1 == '_')
                        GVar3 += 0x27;
                    else if (cVar1 < 'a')
                    {
                        if (cVar1 < 'A')
                        {
                            if (cVar1 < '0')
                            {
                                if (cVar1 == '/')
                                    GVar3 += 2;
                                else if (cVar1 == '-')
                                    GVar3 += 1;
                            }
                            else
                                GVar3 += (ulong)(cVar1 - 0x2D);
                        }
                        else
                            GVar3 += (ulong)(cVar1 - 0x34);
                    }
                    else
                        GVar3 += (ulong)(cVar1 - 0x54);

                    iVar4++;
                    index++;

                    if (iVar4 > 0xB)
                        return GVar3;

                    lVar2 = (long)(GVar3 << 2);
                }
            }

            for (; iVar4 < 0xC; iVar4++)
            {
                GVar3 *= 0x28;
            }

            return GVar3;
        }

    }
}
