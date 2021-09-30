using System;

namespace Common.FileSystemUtilities.Interfaces
{
    public interface IFileStreamHandler
    {
        void Write(byte[] data, string path, bool firstPart);
        
        byte[] Read(string path, long offset, int length);

        void Delete(string path);
    }
}
