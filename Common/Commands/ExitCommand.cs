using System;
using Common.Interfaces;
using Common.Protocol;

namespace Common.Commands
{
    public class ExitCommand : ICommand
    {
        public string Command => CommandConstants.COMMAND_EXIT_CODE;

        // Lo que hace el server.
        public string ActionReq(byte[] payload)
        {
            throw new NotImplementedException();
        }

        // Lo que hace el cliente.
        public VaporStatusMessage ActionRes(byte[] reqPayload)
        {
            throw new NotImplementedException();
        }
    }
}
