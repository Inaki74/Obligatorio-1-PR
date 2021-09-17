using System.Text;
using Common.Interfaces;

namespace Common
{
    public class Payload<T> : IPayload<T>
    {
        public T Info { get; }

        public int Command => throw new System.NotImplementedException();

        public StringPayload(string payloadString)
        {
            Payload = Encoding.UTF8.GetBytes(payloadString);
            Length = payloadString.Length;
        }
        
        
    }
}