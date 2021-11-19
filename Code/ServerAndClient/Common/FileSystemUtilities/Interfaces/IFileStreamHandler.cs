using System;
using System.Threading.Tasks;

namespace Common.FileSystemUtilities.Interfaces
{
    public interface IFileStreamHandler
    {
        Task WriteAsync(byte[] data, string path, bool firstPart);

        Task<byte[]> ReadAsync(string path, long offset, int length);

        void Delete(string path);
    }
}
