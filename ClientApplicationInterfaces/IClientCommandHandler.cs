using System;
using Common.Protocol;

namespace ClientApplicationInterfaces
{
    public interface IClientCommandHandler
    {
        VaporStatusResponse ExecuteCommand(VaporProcessedPacket processedPacket);
    }
}
