using System;
using Common;
using Common.Commands;
using Common.Interfaces;
using Common.NetworkUtilities.Interfaces;
using Common.Protocol;
using Exceptions.BusinessExceptions;
using ServerApplicationInterfaces;

namespace ServerApplication
{
    public class ServerCommandHandler :CommandHandler, IServerCommandHandler
    {
        public CommandResponse ExecuteCommand(VaporProcessedPacket packet)
        {
            ICommand command = DecideCommand(packet.Command);
            

            string response = "";
            try
            {
                response = command.ActionReq(packet.Payload);
            }
            catch(BusinessException be)
            {
                int statusCode = StatusCodeConstants.ERROR_CLIENT;
                response = statusCode.ToString() + be.Message;
            }
            catch(Exception e) //TODO: Ver posibles errores del parte del cliente.
            {
                int statusCode = StatusCodeConstants.ERROR_SERVER;
                response = statusCode.ToString() + $"Something went wrong server-side: {e.Message} + {e.StackTrace}";
            }

            CommandResponse commandResponse = new CommandResponse(response, command.Command);
            
            return commandResponse;
        }
    }
}

