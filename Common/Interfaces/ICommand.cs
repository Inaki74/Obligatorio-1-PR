namespace Common.Interfaces
{
    public interface ICommand
    {
        public int command {get;}

        public VaporPacket ActionReq(IPayload payload);

        public VaporPacket ActionRes(IPayload reqPayload);
    }
}