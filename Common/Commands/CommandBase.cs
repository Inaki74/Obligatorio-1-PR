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
            int statusCode = int.Parse(payloadString.Substring(0, VaporProtocolSpecification.STATUS_CODE_FIXED_SIZE));
            string message = payloadString.Substring(VaporProtocolSpecification.STATUS_CODE_FIXED_SIZE, payloadString.Length-VaporProtocolSpecification.STATUS_CODE_FIXED_SIZE);
            string response = message;

            return new VaporStatusResponse(statusCode, response);
        }
    }
}
