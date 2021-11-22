using System;
using System.Threading.Tasks;
using Configuration;
using Configuration.Interfaces;
using LogCommunicatorInterfaces;
using Models;
using RabbitMQService;
using RabbitMQService.Interfaces;

namespace LogCommunicator
{
    public class LogSender : ILogSender
    {
        private readonly IConfigurationHandler _configurationHandler;
        private readonly IMQStream _streamControl;
        public LogSender()
        {
            _configurationHandler = new ConfigurationHandler();
            _streamControl = RabbitFactory.CreateStream(_configurationHandler.GetField(ConfigurationConstants.HOSTNAME_KEY), _configurationHandler);
        }
        public async Task SendLog(LogModel log)
        {
            Console.WriteLine($"About to send {log.Date}, {log.Description}, {log.Gamename}, {log.Id}, {log.LogType}, {log.Username}");
            
            if(log.LogType == LogType.INFO)
            {
                await _streamControl.SendAsync<LogModel>(_configurationHandler.GetField(ConfigurationConstants.INFO_QUEUENAME_KEY), 
                                               $"info.{log.Username}", log);
            }

            if(log.LogType == LogType.ERROR)
            {
                await _streamControl.SendAsync<LogModel>(_configurationHandler.GetField(ConfigurationConstants.ERROR_QUEUENAME_KEY), 
                                               $"error.{log.Username}", log);
            }
        }
    }
}