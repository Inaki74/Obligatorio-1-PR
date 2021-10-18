using System;
using System.IO;
using System.Net.Sockets;
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

        public byte[] Read(int length)
        {
            int dataReceived = 0;
            var data = new byte[length];
            while (dataReceived < length)
            {
                try
                {
                    var received = _socket.Receive(data, dataReceived, length - dataReceived, SocketFlags.None); //Truncated?
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
                _socket.Send(packet, 0, packet.Length, SocketFlags.None);
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
