using System;
using Common.Commands;
using Common.Protocol;

namespace ServerApplicationInterfaces
{
    public interface IServerCommandHandler
    {
        CommandResponse ExecuteCommand(VaporProcessedPacket packet);
    }
}
