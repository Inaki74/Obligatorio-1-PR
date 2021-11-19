using System;
using Configuration.Interfaces;
using RabbitMQ.Client;
using RabbitMQService.Interfaces;

namespace RabbitMQService
{
    public class RabbitFactory
    {
        private static ConnectionFactory _factory;
        private static IConnection _connection;
        private static IModel _channel;
        
        public static IMQStream CreateStream(string hostName, IConfigurationHandler configurationHandler)
        {
            _factory = new ConnectionFactory{ DispatchConsumersAsync = true, HostName = hostName};
            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();

            return new MQStream(_channel, configurationHandler);
        }
        
        
    }
}