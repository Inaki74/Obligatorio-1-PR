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
        public VaporStatusResponse ExecuteCommand(VaporProcessedPacket processedPacket)
        {
            ICommand command = DecideCommand(processedPacket.Command);
            
            return command.ActionRes(processedPacket.Payload);
        }

    }
}