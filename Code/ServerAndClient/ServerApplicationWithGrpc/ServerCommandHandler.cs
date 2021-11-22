using System;
using Common;
using Common.Commands;
using Common.Interfaces;
using Common.NetworkUtilities.Interfaces;
using Common.Protocol;
using Exceptions.BusinessExceptions;
using LogCommunicatorInterfaces;
using ServerApplicationInterfaces;

namespace ServerApplication
{
    public class ServerCommandHandler :CommandHandler, IServerCommandHandler
    {
        public CommandResponse ExecuteCommand(VaporProcessedPacket packet, ILogSender logSender)
        {
            ICommand command = DecideCommand(packet.Command, logSender);
            

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
            catch(Exception e)
            {
                int statusCode = StatusCodeConstants.ERROR_SERVER;
                response = statusCode.ToString() + $"Something went wrong server-side: {e.Message}";
            }

            CommandResponse commandResponse = new CommandResponse(response, command.Command);
            
            return commandResponse;
        }
    }
}

