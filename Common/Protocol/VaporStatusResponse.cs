using System;

namespace Common.Protocol
{
    public struct VaporStatusResponse
    {
        public readonly int Code { get; }

        public readonly string Message { get; }

        public VaporStatusResponse(int code, string msg)
        {
            Code = code;
            Message = msg;
        }
    }
}
