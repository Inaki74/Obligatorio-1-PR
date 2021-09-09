namespace Common.Protocol.Interfaces
{
    public interface IProtocol
    {
        IProtocolSpecification Specification { get; }
        byte[] CreateHeader(string[] input);
        void Process(byte[] input);
        
    }
}