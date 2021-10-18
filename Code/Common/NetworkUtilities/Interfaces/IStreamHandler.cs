using System;

namespace Common.NetworkUtilities.Interfaces
{
    public interface IStreamHandler
    {
        byte[] Read(int length);
        void Write(byte[] info);
    }
}
