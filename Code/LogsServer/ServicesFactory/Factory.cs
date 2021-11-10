using System;

namespace ServicesFactory
{
    public class Factory
    {
        private readonly IServiceProvider _services;

        public Factory(IServiceProvider services)
        {
            _services = services;
        }

        public void SubscribeServices()
        {
            
        }
    }
}
