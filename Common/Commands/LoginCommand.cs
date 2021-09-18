using System;
using System.Text;
using Business;
using Common.Interfaces;
using Common.NetworkUtilities.Interfaces;
using Common.Protocol;


namespace Common.Commands
{
    public class LoginCommand : Interfaces.ICommand
    {
        //client -> server
        //server -> client

        public string Command => CommandConstants.COMMAND_LOGIN_CODE;

        //Build header, send to server
        public string ActionReq(byte[] payload)
        {
            //Tomas el nombre de usuario y buscan en db
            int statusCode = 0;
            string response = "";

            UserLogic userLogic = new UserLogic();
            try
            {
                bool userExisted = userLogic.Login(Encoding.UTF8.GetString(payload));

                if(userExisted)
                {
                    statusCode = StatusCodeConstants.OK;
                }
                else
                {
                    statusCode = StatusCodeConstants.INFO;
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
        public VaporStatusMessage ActionRes(byte[] payload)
        {
            // XX#XXXX...
            // statusCode#Mensaje
            string payloadString = Encoding.UTF8.GetString(payload);
            int statusCode = int.Parse(payloadString.Substring(0, VaporProtocolSpecification.STATUS_CODE_FIXED_SIZE));
            string message = payloadString.Substring(VaporProtocolSpecification.STATUS_CODE_FIXED_SIZE, payloadString.Length-VaporProtocolSpecification.STATUS_CODE_FIXED_SIZE);
            string response = "";

            switch(statusCode)
            {
                case StatusCodeConstants.OK:
                    response = "Logged in!";
                    break;
                case StatusCodeConstants.INFO:
                    response = "User didn't exist, created new user.";
                    break;
                case StatusCodeConstants.ERROR_CLIENT:
                    response = message;
                    break;
            }

            return new VaporStatusMessage(statusCode, response);
        }
    }
}