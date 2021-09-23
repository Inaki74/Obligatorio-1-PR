using System;
using System.Text;
using Business;
using Common.Interfaces;
using Common.NetworkUtilities.Interfaces;
using Common.Protocol;
using Common.Protocol.NTOs;
using Domain;

namespace Common.Commands
{
    public class LoginCommand : CommandBase, Interfaces.ICommand
    {
        //client -> server
        //server -> client

        public string Command => CommandConstants.COMMAND_LOGIN_CODE;

        //Build header, send to server
        public string ActionReq(byte[] payload)
        {
            //Tomas el nombre de usuario y buscan en db
            UserNetworkTransferObject userDummy = new UserNetworkTransferObject();
            int statusCode = 0;
            string response = "";

            UserLogic userLogic = new UserLogic();
            try
            {
                User user = userDummy.Decode(Encoding.UTF8.GetString(payload));
                bool userExisted = userLogic.Login(user);

                if(userExisted)
                {
                    statusCode = StatusCodeConstants.OK;
                    response = "Logged in!";
                }
                else
                {
                    statusCode = StatusCodeConstants.INFO;
                    response = "User didn't exist, created new user.";
                }
            }
            catch(Exception e)
            {
                //Devolver Codigo de Error del sistema
                statusCode = StatusCodeConstants.ERROR_CLIENT;
                response = $"User already logged in, exception from server: {e.Message}";
            }

            return statusCode.ToString() + response;
        }

        //build the payload for the response
        public VaporStatusResponse ActionRes(byte[] payload)
        {
            VaporStatusResponse statusMessage = ParseStatusResponse(payload);

            return statusMessage;
        }
    }
}