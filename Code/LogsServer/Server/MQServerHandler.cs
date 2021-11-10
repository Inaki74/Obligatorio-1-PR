using System.Threading;
using System.Threading.Tasks;
using BusinessLogicInterfaces;
using Domain;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Models;
using RabbitMQService.Interfaces;
using RabbitMQService;
using Common.Configuration.Interfaces;
using Common.Configuration;

namespace Server
{
    public class MQServerHandler : BackgroundService
    {
        private readonly ILogger<MQServerHandler> _logger;
        private readonly IMQStream _streamControl;
        private readonly ILogLogic _logLogic;
        private readonly IConfigurationHandler _configurationHandler;

        public MQServerHandler(ILogLogic logLogic, IConfigurationHandler configurationHandler)
        {
            _logLogic = logLogic;
            _configurationHandler = configurationHandler;
            _streamControl = RabbitFactory.CreateStream(configurationHandler.GetField(ConfigurationConstants.HOSTNAME_KEY), configurationHandler);
        }
        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _streamControl.ReceiveAsync<LogModel>(
                _configurationHandler.GetField(ConfigurationConstants.INFO_QUEUENAME_KEY), 
                _configurationHandler.GetField(ConfigurationConstants.INFO_ROUTINGKEY_KEY),
                x => Task.Run(() => { _logLogic.Add(x); }, stoppingToken)
            );

            await _streamControl.ReceiveAsync<LogModel>(
                _configurationHandler.GetField(ConfigurationConstants.ERROR_QUEUENAME_KEY),
                _configurationHandler.GetField(ConfigurationConstants.ERROR_ROUTINGKEY_KEY),
                x => Task.Run(() => { _logLogic.Add(x); }, stoppingToken)
            );
        }
        
    }
}