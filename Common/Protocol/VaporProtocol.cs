
namespace Common.Protocol
{
    public class VaporProtocol
    {
        public VaporProtocol()
        {
        }

        public VaporHeader CreateHeader(string requestType, int command, int payloadLength)
        {
            VaporHeader header = new VaporHeader(requestType, command, payloadLength);

            return header;
        }

        public void Process(byte[] input)
        {
            throw new System.NotImplementedException();
        }
    }
}