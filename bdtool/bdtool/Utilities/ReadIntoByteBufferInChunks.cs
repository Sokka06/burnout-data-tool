using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bdtool.Utilities
{
    public abstract class ReadIntoByteBufferInChunks : FileComparer
    {

        protected readonly int ChunkSize;

        protected ReadIntoByteBufferInChunks(string filePath01, string filePath02, int chunkSize) : base(filePath01, filePath02)
        {
            ChunkSize = chunkSize;
        }

        public ReadIntoByteBufferInChunks(FileInfo file01, FileInfo file02, int chunkSize) : base(file01, file02)
        {
            ChunkSize = chunkSize;
        }

        protected int ReadIntoBuffer(in Stream stream, in byte[] buffer)
        {
            var bytesRead = 0;
            while (bytesRead < buffer.Length)
            {
                var read = stream.Read(buffer, bytesRead, buffer.Length - bytesRead);
                // Reached end of stream.
                if (read == 0)
                {
                    return bytesRead;
                }
                bytesRead += read;
            }
            return bytesRead;
        }

    }
}
