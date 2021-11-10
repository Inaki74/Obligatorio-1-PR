using System.Net.Sockets;

namespace Exceptions.ConnectionExceptions
{
    public class EndpointClosedByServerSocketException : ConnectionSocketException
    {
        public EndpointClosedByServerSocketException()
        {
        }

        public override string Message => "Server closed connection";
    }
}