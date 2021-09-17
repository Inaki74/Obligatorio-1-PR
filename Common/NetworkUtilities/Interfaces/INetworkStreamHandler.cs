using System;

namespace Common.NetworkUtilities.Interfaces
{
    public interface INetworkStreamHandler
    {
        byte[] Read();
        void Write(byte[] info);
    }
}
