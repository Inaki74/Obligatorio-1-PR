using Common.Interfaces;

namespace Common
{
    public class StringPayload : IPayload
    {
        public string payload { get; }
        public int length { get; }

        public StringPayload(string payloadString)
        {
            payload = payloadString;
            length = payloadString.Length;
        }
        
        
    }
}