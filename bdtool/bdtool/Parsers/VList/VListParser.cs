using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bdtool.Binary;
using bdtool.Models.B3;
using bdtool.Models.Common;
using YamlDotNet.Core;

namespace bdtool.Parsers.VList
{
    public class VListParser : IParser<Models.Common.VList>
    {
        public virtual Models.Common.VList Read(BinaryReaderE br)
        {
            // Read version number
            var version = br.ReadInt32();

            // Rewind back to start
            br.Seek(0, SeekOrigin.Begin);

            switch (version)
            {
                case 6:
                    return new B3VehicleListParser().Read(br);
                case 9:
                    return new B4VehicleListParser().Read(br);
                default:
                    Console.WriteLine($"No Parser for VList Version '{version}'.");
                    break;
            }

            return null;
        }

        public virtual void Write(BinaryWriterE bw, Models.Common.VList obj)
        {
            switch (obj.VersionNumber)
            {
                case 6:
                    new B3VehicleListParser().Write(bw, obj);
                    break;
                case 9:
                    new B4VehicleListParser().Write(bw, obj);
                    break;
                default:
                    Console.WriteLine($"No Parser for VList Version '{obj.VersionNumber}'.");
                    break;
            }
        }
    }
}
