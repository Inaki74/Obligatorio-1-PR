using System;
using Common.Protocol;

namespace ClientApplicationInterfaces
{
    public interface IClientCommandHandler
    {
        VaporStatusResponse<T> ExecuteCommand<T>(VaporProcessedPacket processedPacket);
    }
}
