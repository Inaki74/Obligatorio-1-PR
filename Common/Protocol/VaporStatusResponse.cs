using System;

namespace Common.Protocol
{
    public class VaporStatusResponse<T>
    {
        public int Code { get; }

        public string Message { get; }

        public T Payload { get; set; } 

        public VaporStatusResponse(int code, string msg)
        {
            Code = code;
            Message = msg;
        }

        public VaporStatusResponse(int code, string msg, T payload)
        {
            Code = code;
            Message = msg;
            Payload = payload;
        }
    }
}
