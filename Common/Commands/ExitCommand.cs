using System;
using System.Text;
using Business;
using Common.Interfaces;
using Common.Protocol;

namespace Common.Commands
{
    public class ExitCommand : CommandBase, ICommand
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
                response = "Logged out.";
            }
            catch(Exception e)
            {
                statusCode = StatusCodeConstants.ERROR_SERVER;
                response = $"Logged out but something went wrong server-side: {e.Message}";
            }

            return statusCode.ToString() + response;
        }

        // Lo que hace el cliente.
        public VaporStatusResponse<T> ActionRes<T>(byte[] payload)
        {
            VaporStatusResponse<T> statusMessage = ParseStatusResponse<T>(payload);

            statusMessage.Payload = default(T);

            return statusMessage;
        }
    }
}
