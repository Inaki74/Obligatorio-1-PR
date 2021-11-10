using System;
using BusinessLogic;
using BusinessLogicInterfaces;
using Common.Configuration;
using Common.Configuration.Interfaces;
using DataAccess;
using DataAccess.Interface;
using Database.Interface;
using Domain;
using Microsoft.Extensions.DependencyInjection;

namespace ServicesFactory
{
    public class Factory
    {
        private readonly IServiceCollection _services;

        public Factory(IServiceCollection services)
        {
            _services = services;
        }

        public void SubscribeServices()
        {
            _services.AddSingleton<ILogLogic, LogLogic>();
            _services.AddSingleton<ILogDataAccess<LogError>, LogErrorDataAccess>();
            _services.AddSingleton<ILogDataAccess<LogInfo>, LogInfoDataAccess>();
            _services.AddSingleton<IDatabase, Database.Database>();
            _services.AddSingleton<IConfigurationHandler, ConfigurationHandler>();
        }
    }
}
