using System;
using System.Text;
using Business;
using Common.Interfaces;
using Common.NetworkUtilities.Interfaces;
using Common.Protocol;
using Common.Protocol.NTOs;
using Domain.BusinessObjects;

namespace Common.Commands
{
    public class LoginCommand : CommandBase, Interfaces.ICommand
    {

        public string Command => CommandConstants.COMMAND_LOGIN_CODE;
        
        public string ActionReq(byte[] payload)
        {
            UserNetworkTransferObject userDummy = new UserNetworkTransferObject();
            UserLogic userLogic = new UserLogic();

            string userString = Encoding.UTF8.GetString(payload);
            User user = userDummy.Decode(userString);
            bool userExisted = userLogic.Login(user);

            int statusCode = 0;
            string response = "";
            
            if(userExisted)
            {
                statusCode = StatusCodeConstants.OK;
                response = user.Username;
            }
            else
            {
                statusCode = StatusCodeConstants.INFO;
                response = "User didn't exist, created new user.";
            }

            return statusCode.ToString() + response;
        }
        
        public VaporStatusResponse ActionRes(byte[] payload)
        {
            VaporStatusResponse statusMessage = ParseStatusResponse(payload);

            return statusMessage;
        }
    }
}