using System.Threading.Channels;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQService.Interfaces;

namespace RabbitMQService
{
    public class MQStream : IMQStream
    {
        private IModel _channel;
        
        public MQStream(IModel channel)
        {
            _channel = channel;
        }
        
        public Task SendAsync<T>()
        {
            throw new System.NotImplementedException();
        }

        public Task ReceiveAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}