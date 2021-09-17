namespace Common.Interfaces
{
    public interface ICommand
    {
        public int command {get;}

        public string ActionReq(byte[] payload);

        public void ActionRes(byte[] reqPayload);
    }
}