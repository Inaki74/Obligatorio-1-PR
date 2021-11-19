using System;
using ClientApplicationInterfaces;
using Common;
using Common.Commands;
using Common.Interfaces;
using Common.NetworkUtilities.Interfaces;
using Common.Protocol;
using LogCommunicatorInterfaces;

namespace ClientApplication
{
    public class ClientCommandHandler : CommandHandler, IClientCommandHandler
    {
        public VaporStatusResponse ExecuteCommand(VaporProcessedPacket processedPacket, ILogSender logSender)
        {
            ICommand command = DecideCommand(processedPacket.Command, logSender);
            
            return command.ActionRes(processedPacket.Payload);
        }

    }
}