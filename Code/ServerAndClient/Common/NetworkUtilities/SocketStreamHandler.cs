using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using Common.NetworkUtilities.Interfaces;
using Exceptions.ConnectionExceptions;

namespace Common.NetworkUtilities
{
    public class SocketStreamHandler : IStreamHandler
    {
        private readonly Socket _socket;
        public SocketStreamHandler(Socket socket)
        {
            _socket = socket;
        }

        public async Task<byte[]> ReadAsync(int length)
        {
            int dataReceived = 0;
            var data = new byte[length];
            while (dataReceived < length)
            {
                try
                {
                    ArraySegment<byte> segment = new ArraySegment<byte>(data, dataReceived, length - dataReceived);
                    
                    int received = await _socket.ReceiveAsync(segment, SocketFlags.None).ConfigureAwait(false); //Truncated?
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
                await _socket.SendAsync(packet, SocketFlags.None);
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
