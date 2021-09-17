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
        private readonly INetworkStreamHandler _networkStreamHandler;

        public LoginCommand(INetworkStreamHandler streamHandler)
        {
            _networkStreamHandler = streamHandler;
        }

        public int command {get;} // al pedo?

        //Build header, send to server
        public void ActionReq(IPayload payload)
        {
            VaporHeader header = new VaporHeader(HeaderConstants.Request, CommandConstants.COMMAND_LOGIN_CODE);
            VaporPacket packet = new VaporPacket(header, payload);

            SendReqLoginPacket(packet);
        }

        //build the payload for the response
        public void ActionRes(IPayload reqPayload)
        {
            // XXXX XXX...XXX Largo Usuario
            //Tomas el nombre de usuario y buscan en db
            UserLogic userLogic = new UserLogic();
            try
            {
                userLogic.Login(Encoding.UTF8.GetString(reqPayload.Payload));
                IPayload payload = new StringPayload("");
                VaporHeader header = new VaporHeader(HeaderConstants.Response, CommandConstants.COMMAND_LOGIN_CODE);
                //VaporHeader = hacer el header
                //IPayload con el codigo de respuesta
                //VaporPacket con los dos anteriores
            }
            catch(Exception e)
            {
                //Devolver Codigo de Error del sistema
            }
            
        }

        private void SendReqLoginPacket(VaporPacket packet)
        {
            // XX XX XXXX XXXX
            // req command largo username
            byte[] packetArray = packet.Create();
            _networkStreamHandler.Write(packetArray);
        }
    }
}