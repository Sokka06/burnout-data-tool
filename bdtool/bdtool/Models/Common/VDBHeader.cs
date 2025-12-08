using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace bdtool.Models.Common
{
    public record VDBHeader
    (
        int Type, // address 0x0
        int DefaultValueCount, // address 0x4
        int Unk1,  // address 0x8. todo: confirm name/purpose.
        int FileDefCount, // address 0xc
        int FileDefOffset // address 0x10
    )
    {
        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.AppendLine($"Type: {Type}");
            builder.AppendLine($"DefaultValueCount: {DefaultValueCount}");
            builder.AppendLine($"Unk1: {Unk1}");
            builder.AppendLine($"FileDefCount: {FileDefCount}");
            builder.AppendLine($"FileDefOffset: {FileDefOffset}");

            return builder.ToString();
        }
    }
}
