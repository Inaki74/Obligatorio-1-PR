using System;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQService.Interfaces;
using Newtonsoft.Json;
using Common.Configuration.Interfaces;
using Common.Configuration;

namespace RabbitMQService
{
    public class MQStream : IMQStream
    {
        private IModel _channel;
        private IConfigurationHandler _configurationHandler;
        
        public MQStream(IModel channel, IConfigurationHandler configurationHandler)
        {
            _channel = channel;
            _configurationHandler = configurationHandler;
        }
        
        public Task SendAsync<T>()
        {
            throw new System.NotImplementedException();
        }

        public async Task ReceiveAsync<T>(string queue, string routingKey, Action<T> onReceive)
        {
            _channel.ExchangeDeclare(exchange: _configurationHandler.GetField(ConfigurationConstants.EXCHANGENAME_KEY), ExchangeType.Topic);
            _channel.QueueDeclare(queue, autoDelete:false, exclusive:false);
            _channel.QueueBind(queue: queue, exchange: _configurationHandler.GetField(ConfigurationConstants.EXCHANGENAME_KEY), routingKey: routingKey);
            
            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.Received += async (s, e) =>
            {
                var body = e.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var item = JsonConvert.DeserializeObject<T>(message);
                onReceive(item);
                await Task.Yield();
            };
            _channel.BasicConsume(queue, true, consumer);
            await Task.Yield();
        }
    }
}