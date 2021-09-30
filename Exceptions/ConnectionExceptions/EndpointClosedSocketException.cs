using System;
using System.Net.Sockets;

namespace Exceptions.ConnectionExceptions
{
    public class EndpointClosedSocketException : ConnectionSocketException
    {
        public EndpointClosedSocketException()
        {
        }

        public override string Message => "Client has disconnected";
    }
}
