using System.Threading.Tasks;
using LogCommunicator.Interfaces;
using Models;

namespace LogCommunicator
{
    public class LogSender : ILogSender
    {
        public async Task SendLog(LogModel log)
        {
            throw new System.NotImplementedException();
            // await rabbit.SendAsync(queuename, algomas, log);
        }
    }
}