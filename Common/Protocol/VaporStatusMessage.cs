using System;

namespace Common.Protocol
{
    public struct VaporStatusMessage
    {
        public readonly int Code { get; }

        public readonly string Message { get; }

        public VaporStatusMessage(int code, string msg)
        {
            Code = code;
            Message = msg;
        }
    }
}
