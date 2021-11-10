using System;
using System.Threading.Tasks;

namespace RabbitMQService.Interfaces
{
    public interface IMQStream
    {
        Task SendAsync<T>(string queueName, string routingKey, T toSend);

        Task ReceiveAsync<T>(string queueName, string routingKey, Action<T> onReceive);
    }
}