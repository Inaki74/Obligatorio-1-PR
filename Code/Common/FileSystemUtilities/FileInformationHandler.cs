using System;
using System.IO;
using Common.FileSystemUtilities.Interfaces;
using Exceptions;

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

            throw new FileReadingException();
        }

        public long GetFileSize(string path)
        {
            if(GetFileExists(path))
            {
                return new FileInfo(path).Length;
            }

            throw new FileReadingException();
        }

    }
}
