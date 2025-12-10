using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace bdtool.Utilities
{
    public class XMLWriter : IDisposable
    {
        private FileStream _file;
        private StreamWriter _writer;
        //private readonly XDocument _doc = new XDocument();
        private int _tabDepth = 0;

        public XMLWriter(string filePath)
        {
            var directory = Path.GetDirectoryName(filePath);
            if (directory == null)
            {
                throw new ArgumentException("Invalid file path");
            }   

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            _file = new FileStream(filePath, FileMode.OpenOrCreate);
            _writer = new StreamWriter(_file);
        }

        public void Create(string fileName)
        {
            _tabDepth = 0;

            if (_writer != null)
            {
                WriteString("<?xml version=\"1.0\"?>");
                var formatted = string.Format("<root name=\"{0}\">", fileName);
                WriteString(formatted);
                ChangeTabDepth(1);
            }
        }

        public void Finish()
        {
            if (_writer != null)
            {
                ChangeTabDepth(-1);
                WriteString("</root>");
                _writer.Close();
                _file.Close();
            }
        }

        public void ChangeTabDepth(int delta)
        {
            _tabDepth = _tabDepth + delta;
        }

        public void OpenSection(string sectionName)
        {
            if (_writer != null)
            {
                var formatted = string.Format("<section name=\"{0}\">", sectionName);
                WriteString(formatted);
                ChangeTabDepth(1);
            }
        }
        public void CloseSection()
        {
            if (_writer != null)
            {
                ChangeTabDepth(-1);
                WriteString("</section>");
            }
        }

        /*public void SaveFileToXML(string filePath)
        {
            _doc.Save(filePath);
        }*/

        public void WriteValue(string valueName, float value)
        {
            if (_writer != null)
            {
                string rawValue = string.Format("0x{0:X8}",BitConverter.SingleToInt32Bits(value));
                string readableValue = string.Format("{0:F}", value);

                WriteStringValue("RwReal", valueName, rawValue, readableValue);
            }
        }

        public void WriteValue(string valueName, int value)
        {
            if (_writer != null)
            {
                string rawValue = string.Format("0x{0:X8}", value);
                string readableValue = string.Format("{0:F}", value);

                WriteStringValue("RwInt32", valueName, rawValue, readableValue);
            }
        }

        public void WriteValue(string valueName, Vector3 value)
        {
            if (_writer != null)
            {
                // check endianess?
                string rawValue = string.Format("0x{0:X8},0x{1:X8},0x{2:X8}",
                    BitConverter.SingleToInt32Bits(value.X),
                    BitConverter.SingleToInt32Bits(value.Y),
                    BitConverter.SingleToInt32Bits(value.Z)
                );

                // Float representation
                string readableValue = string.Format("{0:F},{1:F},{2:F}",value.X, value.Y, value.Z);
                WriteStringValue("GtV3d", valueName, rawValue, readableValue);
            }
        }

        public void WriteStringValue(string typeName, string valueName, string value, string readable)
        {
            string element = string.Format("<{0} name=\"{1}\" value=\"{2}\" readablevalue=\"{3}\"/>",typeName, valueName, value, readable);
            WriteString(element);
        }

        public void WriteString(string xmlString)
        {
            // Write indentation
            for (int i = 0; i < _tabDepth; i++)
            {
                _writer.Write('\t');
            }

            // Write the XML string
            _writer.Write(xmlString);

            // Write newline
            _writer.Write("\r\n");

            _writer.Flush();
        }

        public void Dispose()
        {
            _writer.Dispose();
            _file.Dispose();
        }
    }
}
