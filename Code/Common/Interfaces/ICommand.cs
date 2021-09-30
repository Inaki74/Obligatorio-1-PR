using Common.Protocol;

namespace Common.Interfaces
{
    public interface ICommand
    {
        string Command {get;}

        string ActionReq(byte[] payload);

        VaporStatusResponse ActionRes(byte[] reqPayload);
    }
}