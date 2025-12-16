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
using static bdtool.Dto.VDB;
using static bdtool.Dto.VDB.DefaultValue;

namespace bdtool.Converters
{
    public static class VDBConverter
    {
        public static Dto.VDB ToDto(VDBFile vdb, DatabaseValueDefinitions definitions)
        {
            var defaultValues = new List<DefaultValue>(vdb.DefaultValues.Count);
            var values = new List<VDB.Value>(vdb.Values.Count);
            var fileDefs = new List<FileDef>(vdb.FileDefs.Count);

            // create default values
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
                    value = TypeToValue(definition.Type, entry.Data);
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

            // values
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

                values.Add(new VDB.Value()
                {
                    Address = entry.Address,
                    RawValue = entry.Value.RawValue,
                    Data = data
                });
            }

            // file defs
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

        public static VDBFile FromDto(VDB dto)
        {

            var defaultValues = new List<DatabaseDefaultValue>();
            for (int i = 0; i < dto.DefaultValues.Count; i++)
            {
                var entry = dto.DefaultValues[i];
                defaultValues.Add(new DatabaseDefaultValue { NameHash = entry.NameHash, Data = ValueToData(entry.Value) });
            }

            var values = new List<DatabaseValue>();
            for (int i = 0; i < dto.Values.Count; i++)
            {
                var entry = dto.Values[i];
                //DatabaseValue value = default;
                if (entry.Data is Vector3Value vectorValue)
                {
                    values.Add(new DatabaseValue { Address = entry.Address, Value = new DataElement(vectorValue.Value.X) });
                    values.Add(new DatabaseValue { Address = entry.Address + 4, Value = new DataElement(vectorValue.Value.Y) });
                    values.Add(new DatabaseValue { Address = entry.Address + 8, Value = new DataElement(vectorValue.Value.Z) });
                    values.Add(new DatabaseValue { Address = entry.Address + 12, Value = new DataElement(vectorValue.Value.Padding) });
                } else
                {
                    values.Add(new DatabaseValue { Address = entry.Address, Value = new DataElement(entry.RawValue) });
                }

                /*switch (entry.Data)
                {
                    case IntValue intValue:
                        values.Add(new DatabaseValue { Address = entry.Address, Value = new DataElement(entry.RawValue) });
                        continue;
                    case FloatValue floatValue:
                        values.Add(new DatabaseValue { Address = entry.Address, Value = new DataElement(entry.RawValue) });
                        continue;
                    case BoolValue boolValue:
                        values.Add(new DatabaseValue { Address = entry.Address, Value = new DataElement(entry.RawValue) });
                        continue;
                    case Vector3Value vector3Value:
                        return new DataElement(vector3Value.Value);
                    case PointerValue pointerValue:
                        values.Add(new DatabaseValue { Address = entry.Address, Value = new DataElement(entry.RawValue) });
                        continue;
                    default:
                        value = new DatabaseValue();
                        break;
                }

                if (entry.RawValue != ValueToData(entry.Data).RawValue)
                {
                    ConsoleEx.Warning($"Warning: Deserialized Raw data doesn't match the original!");
                }*/

                //values.Add(new DatabaseValue() { Address = entry.Address, Value = value });
            }

            var fileDefs = new List<DatabaseFileDef>();
            for (int i = 0; i < dto.FileDefs.Count; i++)
            {
                var entry = dto.FileDefs[i];
                fileDefs.Add(new DatabaseFileDef(entry.IsActive, entry.NameHash));
            }


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

        private static DataValue TypeToValue(DataType type, DataElement data)
        {
            switch (type)
            {
                case DataType.None: // no type set
                    return new IntValue() { Type = type, Value = data.RawValue };
                case DataType.RwInt32: // int
                    return new IntValue() { Type = type, Value = data.AsInt() };
                case DataType.RwReal: // float
                    return new FloatValue() { Type = type, Value = data.AsFloat() };
                case DataType.RwBool: // bool
                    return new BoolValue() { Type = type, Value = data.AsBool() };
                case DataType.Pointer: // address to a vector3
                    return new PointerValue() { Type = type, Address = data.RawValue };
                default:
                    return new IntValue() { Type = type, Value = data.RawValue };
            }
        }

        private static DataElement ValueToData(DataValue value)
        {
            switch (value)
            {
                case IntValue intValue:
                    return new DataElement(intValue.Value);
                case FloatValue floatValue:
                    return new DataElement(floatValue.Value);
                case BoolValue boolValue:
                    return new DataElement(boolValue.Value);
                //case Vector3Value vector3Value:
                //    return new DataElement(vector3Value.Value);
                case PointerValue pointerValue:
                    return new DataElement(pointerValue.Address);
                default:
                    return new DataElement();
            }
        }
    }
}
