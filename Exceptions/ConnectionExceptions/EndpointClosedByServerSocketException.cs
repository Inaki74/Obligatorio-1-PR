using System.Net.Sockets;

namespace Exceptions.ConnectionExceptions
{
    public class EndpointClosedByServerSocketException : SocketException
    {
        public EndpointClosedByServerSocketException()
        {
        }

        public override string Message => "Server closed connection";
    }
}