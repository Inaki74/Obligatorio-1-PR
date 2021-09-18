using System;
using System.Text;
using Business;
using Common.Interfaces;
using Common.NetworkUtilities.Interfaces;
using Common.Protocol;
using Domain;

namespace Common.Commands
{
    public class PublishGameCommand : Interfaces.ICommand
    {
        public string Command => CommandConstants.COMMAND_LOGIN_CODE;

        public string ActionReq(byte[] payload)
        {
            //Desarmar payload en un juego. Agregarlo a la lista.
            // Payload = 
            //  Cada field:
            //        XXXX XXXX... - Largo Info
            // Orden: Titulo -> Genero -> Esrb -> Synopsis -> Caratula
            // Armado de juego
            
            Game game = DisassemblePayload(payload);


            // Response
            int statusCode = 0;
            string response = "";

            

            return statusCode.ToString() + response;
        }

        public VaporStatusResponse ActionRes(byte[] payload)
        {
            // Mensaje de si se publico el juego
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

            return new VaporStatusResponse(statusCode, response);
        }

        private Game DisassemblePayload(byte[] payload)
        {
            Game game = new Game();

            
        }
    }
}