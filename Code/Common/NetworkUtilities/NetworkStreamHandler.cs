using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using Common.NetworkUtilities.Interfaces;
using Exceptions.ConnectionExceptions;

namespace Common.NetworkUtilities
{
    public class NetworkStreamHandler : IStreamHandler
    {
        private readonly NetworkStream _stream;
        public NetworkStreamHandler(NetworkStream stream)
        {
            _stream = stream;
        }

        public async Task<byte[]> ReadAsync(int length)
        {
            int dataReceived = 0;
            var data = new byte[length];
            while (dataReceived < length)
            {
                try
                {
                    var received = await _stream.ReadAsync(data, dataReceived, length - dataReceived);
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

        public async Task WriteAsync(byte[] packet)
        {
            try
            {
                await _stream.WriteAsync(packet, 0, packet.Length);
            }
            catch(SocketException se)
            {
                throw new EndpointClosedByServerSocketException();
            }
            catch(IOException e)
            {
                throw new EndpointClosedByServerSocketException();
            }
        }
    }
}
