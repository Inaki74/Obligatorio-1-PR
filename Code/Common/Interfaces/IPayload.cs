namespace Common.Interfaces
{
    public interface IPayload<T>
    {
        public T Info {get;}
        public int Command {get;}
    }
}