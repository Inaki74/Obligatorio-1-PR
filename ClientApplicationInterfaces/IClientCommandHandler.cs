using System;
using Common.Protocol;

namespace ClientApplicationInterfaces
{
    public interface IClientCommandHandler
    {
        VaporStatusMessage ExecuteCommand(VaporProcessedPacket processedPacket);
    }
}
