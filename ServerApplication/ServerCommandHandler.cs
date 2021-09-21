using System;
using Common;
using Common.Commands;
using Common.Interfaces;
using Common.NetworkUtilities.Interfaces;
using Common.Protocol;
using ServerApplicationInterfaces;

namespace ServerApplication
{
    public class ServerCommandHandler :CommandHandler, IServerCommandHandler
    {
        //Get command type and payload from menus input
        //Call ActionReq for desired command with payload as parameterxw
        public CommandResponse ExecuteCommand(VaporProcessedPacket packet)
        {
            ICommand command = DecideCommand(packet.Command);
            

            string response = command.ActionReq(packet.Payload);

            CommandResponse commandResponse = new CommandResponse(response, command.Command);
            
            return commandResponse;
        }
    }
}

