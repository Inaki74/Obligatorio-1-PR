using System.Threading;
using System.Threading.Tasks;
using BusinessLogicInterfaces;
using Domain;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQService.Interfaces;

namespace Server
{
    public class MQServerHandler : BackgroundService
    {
        private readonly ILogger<MQServerHandler> _logger;
        private readonly IMQStream _streamControl;
        private readonly ILogLogic _logLogic;
        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _streamControl.ReceiveAsync<Log>("ayay ?", 
                x => Task.Run(() => { _logLogic.Add(x); }, stoppingToken)
            );

            await _streamControl.ReceiveAsync<Log>("ayay !", 
                x => Task.Run(() => { _logLogic.Add(x); }, stoppingToken)
            );
        }
        
    }
}