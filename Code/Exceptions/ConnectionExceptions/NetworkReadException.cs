using System;
using System.Net.Sockets;

namespace Exceptions.ConnectionExceptions
{
    public class NetworkReadException : SocketException
    {
        public NetworkReadException(){}

        public NetworkReadException(string message)
        {
            _innerMessage = message;
        }
        private string _innerMessage = "";

        public override string Message => $"Something went wrong when reading from network stream.";
    }
}
