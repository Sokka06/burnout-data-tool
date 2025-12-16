using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bdtool.Binary;
using bdtool.Models.VDB;
using bdtool.Utilities;

namespace bdtool.Parsers
{
    public class DatabaseValueParser : IParser<DatabaseValue>
    {

        public DatabaseValue Read(BinaryReaderE br)
        {
            var address = br.Position;
            var value = br.ReadInt32();
            return new Models.VDB.DatabaseValue() { Address = address, Value = new DataElement(value) };
        }

        public void Write(BinaryWriterE bw, DatabaseValue obj)
        {
            Console.WriteLine($"Writing Value at address '{bw.Position}'.");
            if (bw.Position != obj.Address)
            {
                ConsoleEx.Warning($"Warning: Current address doesn't match original address. '{bw.Position}' - '{obj.Address}'");
            }

            bw.WriteInt32(obj.Value.RawValue);
        }
    }
}
