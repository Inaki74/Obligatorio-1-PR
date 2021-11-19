using System;
using Common.Commands;
using Common.Protocol;
using LogCommunicatorInterfaces;

namespace ServerApplicationInterfaces
{
    public interface IServerCommandHandler
    {
        CommandResponse ExecuteCommand(VaporProcessedPacket packet, ILogSender logSender);
    }
}
