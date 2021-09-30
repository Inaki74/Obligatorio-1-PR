using System;
using System.IO;
using Common.FileSystemUtilities.Interfaces;
using Exceptions;

namespace Common.FileSystemUtilities
{
    public class FileStreamHandler : IFileStreamHandler
    {
        public byte[] Read(string path, long offset, int length)
        {
            var data = new byte[length];

            try
            {
                using (var fs = new FileStream(path, FileMode.Open))
                {
                    fs.Position = offset;
                    var bytesRead = 0;
                    while (bytesRead < length)
                    {
                        var read = fs.Read(data, bytesRead, length - bytesRead);
                        if (read == 0)
                        {
                            throw new FileReadingException();   
                        }
                        bytesRead += read;
                    }
                }
            }
            catch(FileReadingException fre)
            {
                throw new FileReadingException(fre.Message);
            }
            catch(Exception e)
            {
                throw new FileReadingException(e.Message);
            }

            return data;
        }

        public void Delete(string path)
        {
            try
            {
                if (File.Exists(path)) File.Delete(path);
            }
            catch(Exception e)
            {
                throw new FileReadingException(e.Message);
            }
        }

        public void Write(byte[] data, string fileName, bool firstPart)
        {
            try
            {
                if (File.Exists(fileName))
                {
                    if (firstPart)
                    {
                        File.Delete(fileName);
                        using (var fs = new FileStream(fileName, FileMode.Create))
                        {
                            fs.Write(data, 0, data.Length);
                        }
                    }
                    else
                    {
                        using (var fs = new FileStream(fileName, FileMode.Append))
                        {
                            fs.Write(data, 0, data.Length);
                        }
                    }
                }
                else
                {
                    System.IO.FileInfo file = new System.IO.FileInfo(fileName);
                    file.Directory.Create();
                    using (var fs = new FileStream(fileName, FileMode.Create))
                    {
                        fs.Write(data, 0, data.Length);
                    }
                }
            }
            catch(Exception e)
            {
                throw new FileWritingException(e.Message);
            }
        }
    }
}
