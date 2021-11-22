using System;

namespace Exceptions.ConnectionExceptions
{
    public class CoverNotReceivedException : Exception
    {
        public override string Message => "Cover was not received!";
    }
}