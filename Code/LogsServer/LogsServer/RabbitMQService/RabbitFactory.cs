using System;
using RabbitMQ.Client;
using RabbitMQService.Interfaces;

namespace RabbitMQService
{
    public class RabbitFactory
    {
        private static ConnectionFactory _factory;
        private static IConnection _connection;
        private static IModel _channel;
        
        private static IMQStream CreateStream(string uri)
        {
            _factory = new ConnectionFactory{ DispatchConsumersAsync = true };
            _factory.Uri = new Uri(uri);
            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();

            return new MQStream(_channel);
        }
        
        
    }
}