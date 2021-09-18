using System;
using Common.Protocol;

namespace ClientApplicationInterfaces
{
    public interface IClientCommandHandler
    {
        void ExecuteCommand(VaporProcessedPacket processedPacket);
    }
}
