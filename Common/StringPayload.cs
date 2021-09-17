using System.Text;
using Common.Interfaces;

namespace Common
{
    public class StringPayload : IPayload
    {
        public byte[] Payload { get; }
        public int Length { get; }

        public StringPayload(string payloadString)
        {
            Payload = Encoding.UTF8.GetBytes(payloadString);
            Length = payloadString.Length;
        }
        
        
    }
}