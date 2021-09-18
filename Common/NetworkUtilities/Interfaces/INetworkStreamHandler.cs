using System;

namespace Common.NetworkUtilities.Interfaces
{
    public interface INetworkStreamHandler
    {
        byte[] Read(int length);
        void Write(byte[] info);
    }
}
