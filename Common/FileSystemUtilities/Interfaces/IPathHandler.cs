using System;

namespace Common.FileSystemUtilities.Interfaces
{
    public interface IPathHandler
    {
        string AppendPath(string path, string toAppend);
    }
}
