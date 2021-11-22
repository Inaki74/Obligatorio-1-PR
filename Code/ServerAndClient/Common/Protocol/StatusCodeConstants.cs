using System;

namespace Common.Protocol
{
    public class StatusCodeConstants
    {
        public const int INFO = 10;
        public const int OK = 20;
        public const int ERROR_CLIENT = 40;
        public const int ERROR_STREAM = 42;
        public const int ERROR_CLIENT_NOTAUTHORIZED = 41;
        public const int ERROR_SERVER = 50;
    }
}
