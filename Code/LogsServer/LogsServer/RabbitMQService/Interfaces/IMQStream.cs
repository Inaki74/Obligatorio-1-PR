using System.Threading.Tasks;

namespace RabbitMQService.Interfaces
{
    public interface IMQStream
    {
        Task SendAsync<T>();

        Task ReceiveAsync();
    }
}