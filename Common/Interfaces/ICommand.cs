namespace Common.Interfaces
{
    public interface ICommand
    {
        public string Command {get;}

        public string ActionReq(byte[] payload);

        public void ActionRes(byte[] reqPayload);
    }
}