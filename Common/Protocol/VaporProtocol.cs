using Common.Protocol.Interfaces;

namespace Common.Protocol
{
    public class VaporProtocol : IProtocol
    {
        
        public VaporProtocol()
        {
            Specification = new VaporProtocolSpecification();
        }

        public IProtocolSpecification Specification { get; }

        public byte[] CreateHeader(string[] input)
        {
            throw new System.NotImplementedException();
        }

        public void Process(byte[] input)
        {
            throw new System.NotImplementedException();
        }
    }
}