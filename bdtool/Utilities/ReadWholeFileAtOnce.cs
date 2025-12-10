using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bdtool.Utilities
{
    public class ReadWholeFileAtOnce : FileComparer
    {

        public ReadWholeFileAtOnce(string filePath01, string filePath02) : base(filePath01, filePath02)
        {
        }

        public ReadWholeFileAtOnce(FileInfo file01, FileInfo file02) : base(file01, file02)
        {
        }

        protected override bool OnCompare(out long position)
        {
            position = 0;

            var fileContents01 = File.ReadAllBytes(FileInfo1.FullName);
            var fileContents02 = File.ReadAllBytes(FileInfo2.FullName);
            for (var i = 0; i < fileContents01.Length; i++)
            {
                position = i;
                if (fileContents01[i] != fileContents02[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}
