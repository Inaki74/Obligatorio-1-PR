using System;
using Business;
using Common.Interfaces;
using Common.Protocol;


namespace Common.Commands
{
    public class LoginCommand : Interfaces.ICommand
    {
        public int command {get;}

        //Build header, send to server
        public VaporPacket ActionReq(IPayload payload)
        {
            VaporHeader header = new VaporHeader(HeaderConstants.Request, CommandConstants.COMMAND_LOGIN_CODE, payload.length);
            VaporPacket packet = new VaporPacket(header, payload);
            return packet;
        }

        //build the payload for the response
        public VaporPacket ActionRes(IPayload reqPayload)
        {
            // XXXX XXX...XXX Largo Usuario
            //Tomas el nombre de usuario y buscan en db
            UserLogic userLogic = new UserLogic();
            try
            {
                userLogic.Login(reqPayload.payload);
                IPayload payload = new StringPayload("");
                VaporHeader header = new VaporHeader(HeaderConstants.Response, CommandConstants.COMMAND_LOGIN_CODE,
                    payload.length);
                //VaporHeader = hacer el header
                //IPayload con el codigo de respuesta
                //VaporPacket con los dos anteriores
            }
            catch(Exception e)
            {
                //Devolver Codigo de Error del sistema
            }
            
        }
    }
}