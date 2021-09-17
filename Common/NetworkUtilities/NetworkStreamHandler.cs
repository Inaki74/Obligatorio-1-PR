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

        public byte[] Read(int length)
        {
            int dataReceived = 0;
            var data = new byte[length];
            while (dataReceived < length)
            {
                var received = _stream.Read(data, dataReceived, length - dataReceived);
                if (received == 0)
                {
                    throw new SocketException();
                }
                dataReceived += received;
            }

            return data;
        }

        public void Write(byte[] packet)
        {
            _stream.Write(packet, 0, packet.Length);
        }
    }
}
