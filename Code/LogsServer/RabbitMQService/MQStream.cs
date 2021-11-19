using System;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQService.Interfaces;
using Newtonsoft.Json;
using Configuration;
using Configuration.Interfaces;

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
        
        public async Task SendAsync<T>(string queueName, string routingKey, T toSend)
        {
            await Task.Run(() =>
            {
                string exchangeName = _configurationHandler.GetField(ConfigurationConstants.EXCHANGENAME_KEY);

                _channel.ExchangeDeclare(exchange: exchangeName, ExchangeType.Topic);
                _channel.QueueDeclare(queueName, autoDelete:false, exclusive:false);
                _channel.QueueBind(queue: queueName, exchange: exchangeName, routingKey: routingKey);

                var properties = _channel.CreateBasicProperties();
                properties.Persistent = false;

                var output = JsonConvert.SerializeObject(toSend);
                _channel.BasicPublish(exchangeName, queueName, null, Encoding.UTF8.GetBytes(output));
            });
        }

        public async Task ReceiveAsync<T>(string queueName, string routingKey, Action<T> onReceive)
        {
            string exchangeName = _configurationHandler.GetField(ConfigurationConstants.EXCHANGENAME_KEY);

            _channel.ExchangeDeclare(exchange: exchangeName, ExchangeType.Topic);
            _channel.QueueDeclare(queueName, autoDelete:false, exclusive:false);
            _channel.QueueBind(queue: queueName, exchange: exchangeName, routingKey: routingKey);
            
            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.Received += async (s, e) =>
            {
                var body = e.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var item = JsonConvert.DeserializeObject<T>(message);
                onReceive(item);
                await Task.Yield();
            };
            
            _channel.BasicConsume(queueName, true, consumer);
            await Task.Yield();
        }
    }
}