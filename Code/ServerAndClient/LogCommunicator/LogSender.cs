using System;
using System.Threading.Tasks;
using LogCommunicator.Interfaces;
using Models;

namespace LogCommunicator
{
    public class LogSender : ILogSender
    {
        public async Task SendLog(LogModel log)
        {
            Console.WriteLine($"About to send {log.Date}, {log.Description}, {log.Gamename}, {log.Id}, {log.LogType}, {log.Username}");
            // await rabbit.SendAsync(queuename, algomas, log);
        }
    }
}