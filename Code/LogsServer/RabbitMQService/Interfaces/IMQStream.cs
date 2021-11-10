using System;
using System.Threading.Tasks;

namespace RabbitMQService.Interfaces
{
    public interface IMQStream
    {
        Task SendAsync<T>();

        Task ReceiveAsync<T>(string queueName, string routingKey, Action<T> onReceive);
    }
}