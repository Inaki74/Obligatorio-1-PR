using System;
using System.Net.Sockets;

namespace Exceptions.ConnectionExceptions
{
    public class ConnectionSocketException : SocketException
    {
        public ConnectionSocketException()
        {
        }

        public override string Message => "Something went wrong with the connection.";
    }
}
