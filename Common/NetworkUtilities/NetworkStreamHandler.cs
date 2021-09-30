using System;
using System.IO;
using System.Net.Sockets;
using Common.NetworkUtilities.Interfaces;
using Exceptions.ConnectionExceptions;

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
                try
                {
                    var received = _stream.Read(data, dataReceived, length - dataReceived);
                    if (received == 0)
                    {
                        throw new EndpointClosedSocketException();
                    }
                    dataReceived += received;
                }
                catch (IOException ioe)
                {
                    throw new EndpointClosedByServerSocketException();
                }
                catch (Exception e)
                {
                    throw new NetworkReadException(e.Message);
                }
                
            }

            return data;
        }

        public void Write(byte[] packet)
        {
            try
            {
                _stream.Write(packet, 0, packet.Length);
            }
            catch(SocketException se)
            {
                throw new EndpointClosedByServerSocketException();
            }
        }
    }
}
