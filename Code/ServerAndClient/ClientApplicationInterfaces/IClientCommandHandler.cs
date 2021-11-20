using System;
using Common.Protocol;
using LogCommunicatorInterfaces;

namespace ClientApplicationInterfaces
{
    public interface IClientCommandHandler
    {
        VaporStatusResponse ExecuteCommand(VaporProcessedPacket processedPacket, ILogSender logSender);
    }
}
