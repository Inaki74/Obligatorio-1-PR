using System;
using System.Text;
using Business;
using Common.Interfaces;
using Common.Protocol;
using Common.Protocol.NTOs;
using Domain.BusinessObjects;

namespace Common.Commands
{
    public class ExitCommand : CommandBase, ICommand
    {
        public string Command => CommandConstants.COMMAND_EXIT_CODE;

        // Lo que hace el server.
        public string ActionReq(byte[] payload)
        {
            UserNetworkTransferObject userDummy = new UserNetworkTransferObject();
            int statusCode = 0;
            string response = "";

            UserLogic userLogic = new UserLogic();
            try
            {
                User user = userDummy.Decode(Encoding.UTF8.GetString(payload));
                userLogic.Logout(user);
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
        public VaporStatusResponse ActionRes(byte[] payload)
        {
            VaporStatusResponse statusMessage = ParseStatusResponse(payload);

            return statusMessage;
        }
    }
}
