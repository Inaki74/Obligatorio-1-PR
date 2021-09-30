using System;
using System.Net.Sockets;

namespace Exceptions.ConnectionExceptions
{
    public class ExitException : SocketException
    {
        public override string Message => "Connection closed, leaving application.";
    }
}
