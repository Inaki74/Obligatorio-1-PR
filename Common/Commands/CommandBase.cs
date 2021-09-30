using System;
using System.Text;
using Common.Protocol;

namespace Common.Commands
{
    public abstract class CommandBase
    {
        protected virtual VaporStatusResponse ParseStatusResponse(byte[] payload)
        {
            string payloadString = Encoding.UTF8.GetString(payload);
            int statusCodeFixedSize = VaporProtocolSpecification.STATUS_CODE_FIXED_SIZE;
            int statusCode = int.Parse(payloadString.Substring(0, VaporProtocolSpecification.STATUS_CODE_FIXED_SIZE));
            string message = payloadString.Substring(statusCodeFixedSize, payloadString.Length-statusCodeFixedSize);
            string response = message;

            return new VaporStatusResponse(statusCode, response);
        }
    }
}
