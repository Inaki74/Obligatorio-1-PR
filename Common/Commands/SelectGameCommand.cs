using Common.Protocol;

namespace Common.Commands
{
    public class SelectGameCommand : CommandBase, Interfaces.ICommand
    {
        public string Command { get; }
        public string ActionReq(byte[] payload)
        {
            throw new System.NotImplementedException();
        }

        public VaporStatusResponse ActionRes(byte[] reqPayload)
        {
            throw new System.NotImplementedException();
        }
    }
}