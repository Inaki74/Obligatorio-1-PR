using Common.Interfaces;
using Common.Protocol;

namespace Common
{
    public class VaporPacket
    {
        public VaporHeader header;
        public IPayload payload;

        public VaporPacket(VaporHeader header, IPayload payload)
        {
            this.header = header;
            this.payload = payload;
        }
        
        
    }
}