using System;
using System.IO;
using Common.FileSystemUtilities.Interfaces;

namespace Common.FileSystemUtilities
{
    public class FileInformationHandler : IFileInformationHandler
    {
        public bool GetFileExists(string path)
        {
            return File.Exists(path);
        }

        public string GetFileName(string path)
        {
            if(GetFileExists(path))
            {
                return new FileInfo(path).Name;
            }

            throw new Exception("No such path.");
        }

        public long GetFileSize(string path)
        {
            if(GetFileExists(path))
            {
                return new FileInfo(path).Length;
            }

            throw new Exception("No such path.");
        }
    }
}
