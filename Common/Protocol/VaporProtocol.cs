
namespace Common.Protocol
{
    public class VaporProtocol
    {
        public VaporProtocol()
        {
        }

        public VaporHeader CreateHeader(string header, int command, int payloadType, int payloadLength)
        {
            VaporHeader header = new VaporHeader(header, command, payloadType, payloadLength);

            return header;
        }

        public void Process(byte[] input)
        {
            throw new System.NotImplementedException();
        }
    }
}