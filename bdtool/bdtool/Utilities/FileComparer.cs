using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bdtool.Utilities
{
    public abstract class FileComparer
    {

        /// <summary>
        /// Fileinfo for source file
        /// </summary>
        protected readonly FileInfo FileInfo1;

        /// <summary>
        /// Fileinfo for target file
        /// </summary>
        protected readonly FileInfo FileInfo2;

        /// <summary>
        /// Base class for creating a file comparer
        /// </summary>
        /// <param name="filePath01">Absolute path to source file</param>
        /// <param name="filePath02">Absolute path to target file</param>
        protected FileComparer(string filePath01, string filePath02)
        {
            FileInfo1 = new FileInfo(filePath01);
            FileInfo2 = new FileInfo(filePath02);
            EnsureFilesExist();
        }

        /// <summary>
        /// Base class for creating a file comparer
        /// </summary>
        /// <param name="filePath01">Absolute path to source file</param>
        /// <param name="filePath02">Absolute path to target file</param>
        protected FileComparer(FileInfo file01, FileInfo file02)
        {
            FileInfo1 = file01;
            FileInfo2 = file02;
            EnsureFilesExist();
        }

        /// <summary>
        /// Compares the two given files and returns true if the files are the same
        /// </summary>
        /// <returns>true if the files are the same, false otherwise</returns>
        public bool Compare(out long position)
        {
            position = -1;
            /*if (IsDifferentLength())
            {
                return false;
            }*/

            // 
            /*if (IsSameFile())
            {
                return true;
            }*/

            return OnCompare(out position);
        }

        /// <summary>
        /// Compares the two given files and returns true if the files are the same
        /// </summary>
        /// <returns>true if the files are the same, false otherwise</returns>
        protected abstract bool OnCompare(out long position);

        private bool IsSameFile()
        {
            return string.Equals(FileInfo1.FullName, FileInfo2.FullName, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Does an early comparison by checking files Length, if lengths are not the same, files are definetely different
        /// </summary>
        /// <returns>true if different length</returns>
        private bool IsDifferentLength()
        {
            return FileInfo1.Length != FileInfo2.Length;
        }

        /// <summary>
        /// Makes sure files exist
        /// </summary>
        private void EnsureFilesExist()
        {
            if (FileInfo1.Exists == false)
            {
                throw new ArgumentNullException(nameof(FileInfo1));
            }
            if (FileInfo2.Exists == false)
            {
                throw new ArgumentNullException(nameof(FileInfo2));
            }
        }

    }
}
