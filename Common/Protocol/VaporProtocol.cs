
namespace Common.Protocol
{
    public class VaporProtocol
    {
        public VaporProtocol()
        {
        }

        public VaporHeader CreateHeader(string requestType, int command)
        {
            VaporHeader header = new VaporHeader(requestType, command);

            return header;
        }

        public void Process(byte[] input)
        {
            throw new System.NotImplementedException();
        }
    }
}