using Common.Protocol.Interfaces;

namespace Common.Protocol
{
    public class VaporProtocolSpecification : IProtocolSpecification
    {
        public int MaxPacketSize => 16382;
    }
}