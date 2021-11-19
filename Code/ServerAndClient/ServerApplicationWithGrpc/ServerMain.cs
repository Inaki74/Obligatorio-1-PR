using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using ServerApplicationInterfaces;

namespace ServerApplication
{
    class ServerMain : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            new ServerHandler();

            bool serverStarted = await IServerHandler.Instance.StartServerAsync();

            if (serverStarted)
            {
                await IServerHandler.Instance.StartClientListeningTaskAsync(stoppingToken);
            }
            
        }

        
    }
}
