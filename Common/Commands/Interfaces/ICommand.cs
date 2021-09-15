namespace Common.Commands.Interfaces
{
    public interface ICommand
    {
        public int command {get;}
        
        public void ActionReq(IPayload payload)
        {
        }
        
        public void ActionRes()
        {
        }
    }
}