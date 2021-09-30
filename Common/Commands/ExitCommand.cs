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
        
        public string ActionReq(byte[] payload)
        {
            UserNetworkTransferObject userDummy = new UserNetworkTransferObject();
            int statusCode = 0;
            string response = "";
            UserLogic userLogic = new UserLogic();
            User user = userDummy.Decode(Encoding.UTF8.GetString(payload));
            userLogic.Logout(user);
            statusCode = StatusCodeConstants.OK;
            response = "Logged out.";

            return statusCode.ToString() + response;
        }
        
        public VaporStatusResponse ActionRes(byte[] payload)
        {
            VaporStatusResponse statusMessage = ParseStatusResponse(payload);

            return statusMessage;
        }
    }
}
