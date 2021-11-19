using System;
using System.Text;
using Business;
using Common.Interfaces;
using Common.Protocol;
using Common.Protocol.NTOs;
using Domain.BusinessObjects;
using LogCommunicatorInterfaces;

namespace Common.Commands
{
    public class ExitCommand : CommandBase, ICommand
    {
        public ExitCommand(ILogSender logSender) : base(logSender)
        {
        }

        public string Command => CommandConstants.COMMAND_EXIT_CODE;
        
        public string ActionReq(byte[] payload)
        {
            UserNetworkTransferObject userDummy = new UserNetworkTransferObject();
            UserLogic userLogic = new UserLogic();
            
            int statusCode = 0;
            string response = "";

            string userString = Encoding.UTF8.GetString(payload);
            User user = userDummy.Decode(userString);
            
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
