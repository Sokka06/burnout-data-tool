using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bdtool.Binary;
using bdtool.Models.Common;
using bdtool.Parsers.VList;
using bdtool.Utilities;

namespace bdtool.Parsers.VData
{
    public class VehicleDataParser : IParser<VehicleData>
    {
        public virtual VehicleData Read(BinaryReaderE br)
        {
            // Read version number
            var version = br.ReadInt32();

            // Rewind back to start
            br.Seek(0, SeekOrigin.Begin);

            switch (version)
            {
                case 21:
                    return new B3VehicleDataParser().Read(br);
                case 23:
                    return new B3VehicleDataParser().Read(br);
                case 29:
                    //return new B4VehicleDataParser().Read(br);
                    break;
                default:
                    ConsoleEx.Error($"No Parser for Vehicle Data Version '{version}'.");
                    break;
            }

            throw new NotImplementedException();
        }

        public virtual void Write(BinaryWriterE bw, VehicleData obj)
        {
            switch (obj.VersionNumber)
            {
                case 23:
                    new B3VehicleDataParser().Write(bw, obj);
                    break;
                case 29:
                    //new B4VehicleDataParser().Write(bw, obj);
                    break;
                default:
                    ConsoleEx.Error($"No Parser for Vehicle Data Version '{obj.VersionNumber}'.");
                    throw new NotImplementedException();
            }
        }
    }
}
