using System;
using ClientApplicationInterfaces;
using Common;
using Common.Commands;
using Common.Interfaces;
using Common.NetworkUtilities.Interfaces;
using Common.Protocol;

namespace ClientApplication
{
    public class ClientCommandHandler : CommandHandler, IClientCommandHandler
    {
        //Get command type and payload from menus input
        //Call ActionReq for desired command with payload as parameter
        
        public VaporStatusResponse<T> ExecuteCommand<T>(VaporProcessedPacket processedPacket)
        {
            ICommand command = DecideCommand(processedPacket.Command);
            
            return command.ActionRes<T>(processedPacket.Payload);
        }
        
        

    }
}