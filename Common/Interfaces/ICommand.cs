using Common.Protocol;

namespace Common.Interfaces
{
    public interface ICommand
    {
        public string Command {get;}

        public string ActionReq(byte[] payload);

        public VaporStatusResponse<T> ActionRes<T>(byte[] reqPayload);
    }
}