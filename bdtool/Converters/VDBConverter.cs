using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using bdtool.Definitions;
using bdtool.Dto;
using bdtool.Models.B3;
using bdtool.Models.B4;
using bdtool.Models.Types;
using bdtool.Models.VDB;
using bdtool.Utilities;
using YamlDotNet.Core.Tokens;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static bdtool.Dto.VDB;
using static bdtool.Dto.VDB.DefaultValue;

namespace bdtool.Converters
{
    public static class VDBConverter
    {
        /// <summary>
        /// Convert raw VDB to Dto.
        /// </summary>
        /// <param name="vdb"></param>
        /// <param name="definitions"></param>
        /// <returns></returns>
        public static Dto.VDB ToDto(VDBFile vdb, DatabaseValueDefinitions definitions)
        {
            var defaultValues = new List<DefaultValue>(vdb.DefaultValues.Count);
            var values = new List<VDB.Value>(vdb.Values.Count);
            var fileDefs = new List<FileDef>(vdb.FileDefs.Count);

            // DEFAULT VALUES
            for (int i = 0; i < vdb.DefaultValues.Count; i++)
            {
                var entry = vdb.DefaultValues[i];

                var name = "";
                var path = "";
                var fileName = "";
                DataValue value = new IntValue
                {
                    Type = 0,
                    Value = entry.Data.RawValue
                };

                // find matching definition file.
                if (definitions.DefaultValues.TryGetValue(entry.NameHash, out var definition))
                {
                    switch (definition.Type)
                    {
                        case DataType.RwReal: // float
                            value = new FloatValue() { Type = definition.Type, Value = entry.Data.AsFloat() };
                            break;
                        case DataType.RwBool: // bool
                            value = new BoolValue() { Type = definition.Type, Value = entry.Data.AsBool() };
                            break;
                        case DataType.Pointer: // address to a value
                            value = new PointerValue() { Type = definition.Type, Address = entry.Data.RawValue };
                            break;
                        default:
                            value = new IntValue() { Type = definition.Type, Value = entry.Data.RawValue };
                            break;
                    }
                    name = definition.Name;
                    path = definition.Path;
                    fileName = definition.FileName;
                }

                defaultValues.Add(new DefaultValue
                {
                    NameHash = entry.NameHash,
                    Value = value,
                    Name = name,
                    Path = path,
                    FileName = fileName,
                });
            }

            // VALUES
            for (int i = 0; i < vdb.Values.Count; i++)
            {
                var entry = vdb.Values[i];

                DataValue data = new IntValue
                {
                    Type = 0,
                    Value = entry.Value.RawValue
                };

                // find matching definition.
                if (definitions.Values.TryGetValue(entry.Address, out var definition))
                {
                    switch (definition.Type)
                    {
                        case DataType.None:
                            data = new IntValue { Type = definition.Type, Value = entry.Value.AsInt() };
                            break;
                        case DataType.RwInt32:
                            data = new IntValue { Type = definition.Type, Value = entry.Value.AsInt() };
                            break;
                        case DataType.RwReal:
                            data = new FloatValue { Type = definition.Type, Value = entry.Value.AsFloat() };
                            break;
                        case DataType.RwBool:
                            data = new BoolValue { Type = definition.Type, Value = entry.Value.AsBool() };
                            break;
                        case DataType.V3d:
                            // consume next 4 values.
                            var x = vdb.Values[i].Value.AsFloat();
                            var y = vdb.Values[i + 1].Value.AsFloat();
                            var z = vdb.Values[i + 2].Value.AsFloat();
                            var p = vdb.Values[i + 3].Value.AsFloat();
                            i += 3;

                            data = new Vector3Value { Type = definition.Type, Value = new V3d { X = x, Y = y, Z = z, Padding = p } };
                            break;
                        case DataType.Pointer:
                            data = new PointerValue { Type = definition.Type, Address = entry.Value.RawValue };
                            break;
                        default:
                            data = new IntValue { Type = definition.Type, Value = entry.Value.AsInt() };
                            break;
                    }
                }

                values.Add(new VDB.Value
                {
                    Address = entry.Address,
                    //RawValue = entry.Value.RawValue,
                    Data = data
                });
            }

            // FILE DEFS
            for (int i = 0; i < vdb.FileDefs.Count; i++)
            {
                var entry = vdb.FileDefs[i];

                var fileName = "";

                // find matching definition.
                if (definitions.Files.TryGetValue(entry.FileHash, out var definition))
                {
                    fileName = definition.FileName;
                }

                fileDefs.Add(new FileDef()
                {
                    NameHash = entry.FileHash,
                    FileName = fileName,
                    IsActive = entry.IsActive
                });
            }

            return new VDB() 
            {
                Type = vdb.Header.Type,
                Unk1 = vdb.Header.Unk1,
                DefaultValues = defaultValues,
                Values = values,
                FileDefs = fileDefs
            };
        }

        /// <summary>
        /// Convert Dto VDB to raw.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public static VDBFile FromDto(VDB dto)
        {
            // DEFAULT VALUES
            var defaultValues = new List<DatabaseDefaultValue>();
            for (int i = 0; i < dto.DefaultValues.Count; i++)
            {
                var entry = dto.DefaultValues[i];

                DataElement data = new DataElement();
                switch (entry.Value)
                {
                    case IntValue intValue:
                        data = new DataElement(intValue.Value);
                        break;
                    case FloatValue floatValue:
                        data = new DataElement(floatValue.Value);
                        break;
                    case BoolValue boolValue:
                        data = new DataElement(boolValue.Value);
                        break;
                    //case Vector3Value vector3Value:
                    //    return new DataElement(vector3Value.Value);
                    case PointerValue pointerValue:
                        data = new DataElement(pointerValue.Address);
                        break;
                    default:
                        break;
                }

                defaultValues.Add(new DatabaseDefaultValue { NameHash = entry.NameHash, Data = data });
            }

            // VALUES
            var values = new List<DatabaseValue>();
            for (int i = 0; i < dto.Values.Count; i++)
            {
                var entry = dto.Values[i];
                switch (entry.Data)
                {
                    case IntValue intValue:
                        values.Add(new DatabaseValue { Address = entry.Address, Value = new DataElement(intValue.Value) });
                        break;
                    case FloatValue floatValue:
                        values.Add(new DatabaseValue { Address = entry.Address, Value = new DataElement(floatValue.Value) });
                        break;
                    case BoolValue boolValue:
                        values.Add(new DatabaseValue { Address = entry.Address, Value = new DataElement(boolValue.Value) });
                        break;
                    case PointerValue pointerValue:
                        values.Add(new DatabaseValue { Address = entry.Address, Value = new DataElement(pointerValue.Address) });
                        break;
                    case Vector3Value vector3Value:
                        // unpack the vector3 to float values
                        values.Add(new DatabaseValue { Address = entry.Address, Value = new DataElement(vector3Value.Value.X) });
                        values.Add(new DatabaseValue { Address = entry.Address + 4, Value = new DataElement(vector3Value.Value.Y) });
                        values.Add(new DatabaseValue { Address = entry.Address + 8, Value = new DataElement(vector3Value.Value.Z) });
                        values.Add(new DatabaseValue { Address = entry.Address + 12, Value = new DataElement(vector3Value.Value.Padding) });
                        break;
                    default:
                        throw new Exception();
                }
                
                /*if (entry.Data is Vector3Value vectorValue)
                {
                    // unpack the vector3 to float values
                    values.Add(new DatabaseValue { Address = entry.Address, Value = new DataElement(vectorValue.Value.X) });
                    values.Add(new DatabaseValue { Address = entry.Address + 4, Value = new DataElement(vectorValue.Value.Y) });
                    values.Add(new DatabaseValue { Address = entry.Address + 8, Value = new DataElement(vectorValue.Value.Z) });
                    values.Add(new DatabaseValue { Address = entry.Address + 12, Value = new DataElement(vectorValue.Value.Padding) });
                } else
                {
                    values.Add(new DatabaseValue { Address = entry.Address, Value = new DataElement(entry.RawValue) });
                }*/
            }

            // FILE DEFS
            var fileDefs = new List<DatabaseFileDef>();
            for (int i = 0; i < dto.FileDefs.Count; i++)
            {
                var entry = dto.FileDefs[i];
                fileDefs.Add(new DatabaseFileDef(entry.IsActive, entry.NameHash));
            }

            // HEADER
            var fileDefOffset =
                VDBHeader.HEADER_LENGTH +
                DatabaseDefaultValue.DEFAULT_VALUE_LENGTH * defaultValues.Count +
                DatabaseValue.VALUE_LENGTH * values.Count;

            var header = new VDBHeader()
            {
                Type = dto.Type,
                DefaultValueCount = defaultValues.Count,
                Unk1 = dto.Unk1,
                FileDefCount = dto.FileDefs.Count,
                FileDefOffset = fileDefOffset
            };

            return new VDBFile() 
            {
                Header = header,
                DefaultValues = defaultValues,
                Values = values,
                FileDefs = fileDefs
            };
        }
    }
}
