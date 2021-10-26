using System;
using System.Threading.Tasks;

namespace Common.NetworkUtilities.Interfaces
{
    public interface IStreamHandler
    {
        Task<byte[]> ReadAsync(int length);
        Task WriteAsync(byte[] info);
    }
}
