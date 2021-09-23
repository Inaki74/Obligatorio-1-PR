using System;

namespace Common.FileSystemUtilities.Interfaces
{
    public interface IFileInformationHandler
    {
        bool GetFileExists(string path);
        string GetFileName(string path);
        long GetFileSize(string path);
    }
}
