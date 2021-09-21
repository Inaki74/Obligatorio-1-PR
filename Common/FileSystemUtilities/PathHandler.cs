using System;
using Common.FileSystemUtilities.Interfaces;
using System.IO;

namespace Common.FileSystemUtilities
{
    public class PathHandler : IPathHandler
    {
        public string AppendPath(string path, string toAppend)
        {
            return Path.Combine(path, toAppend);
        }
    }
}
