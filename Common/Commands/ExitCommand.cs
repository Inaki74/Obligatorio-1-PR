using System;
using System.Text;
using Business;
using Common.Interfaces;
using Common.Protocol;

namespace Common.Commands
{
    public class ExitCommand : ICommand
    {
        public string Command => CommandConstants.COMMAND_EXIT_CODE;

        // Lo que hace el server.
        public string ActionReq(byte[] payload)
        {
            int statusCode = 0;
            string response = "";

            UserLogic userLogic = new UserLogic();
            try
            {
                userLogic.Logout(Encoding.UTF8.GetString(payload));
                statusCode = StatusCodeConstants.OK;
            }
            catch(Exception e)
            {
                statusCode = StatusCodeConstants.ERROR_SERVER;
                response = $"Logged out but something went wrong server-side: {e.Message}";
            }

            return statusCode.ToString() + response;
        }

        // Lo que hace el cliente.
        public VaporStatusMessage ActionRes(byte[] payload)
        {
            string payloadString = Encoding.UTF8.GetString(payload);
            int statusCode = int.Parse(payloadString.Substring(0, VaporProtocolSpecification.STATUS_CODE_FIXED_SIZE));
            string message = payloadString.Substring(VaporProtocolSpecification.STATUS_CODE_FIXED_SIZE, payloadString.Length-VaporProtocolSpecification.STATUS_CODE_FIXED_SIZE);
            string response = "";

            switch(statusCode)
            {
                case StatusCodeConstants.OK:
                    response = "Logged out.";
                    break;
                case StatusCodeConstants.ERROR_SERVER:
                    response = message;
                    break;
            }

            return new VaporStatusMessage(statusCode, response);
        }
    }
}
