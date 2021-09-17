using System;
using System.Net.Sockets;
using Common.NetworkUtilities.Interfaces;

namespace Common.NetworkUtilities
{
    public class NetworkStreamHandler : INetworkStreamHandler
    {
        private readonly NetworkStream _stream;
        public NetworkStreamHandler(NetworkStream stream)
        {
            _stream = stream;
        }

        public byte[] Read()
        {
            return null;
        }

        public void Write(byte[] packet)
        {

        }
    }
}
