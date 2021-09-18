using Common.Protocol;

namespace Common.Interfaces
{
    public interface ICommand
    {
        public string Command {get;}

        public string ActionReq(byte[] payload);

        public VaporStatusMessage ActionRes(byte[] reqPayload);
    }
}