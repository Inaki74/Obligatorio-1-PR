using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using ServerApplicationInterfaces;
using BusinessInterfaces;
using LogCommunicatorInterfaces;

namespace ServerApplication
{
    class ServerMain : BackgroundService
    {
        public ServerMain(ILogSender logSender)
        {
            new ServerHandler(logSender);
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            bool serverStarted = await IServerHandler.Instance.StartServerAsync();

            if (serverStarted)
            {
                await IServerHandler.Instance.StartClientListeningTaskAsync(stoppingToken);
            }
            
        }

        
    }
}
