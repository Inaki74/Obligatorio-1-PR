using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQService.Interfaces;

namespace Server
{
    public class MQServerHandler : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IMQStream _streamControl;
        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            
        }
        
    }
}