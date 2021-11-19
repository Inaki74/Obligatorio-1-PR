using System;
using System.Threading.Tasks;
using Models;

namespace LogCommunicatorInterfaces
{
    public interface ILogSender
    {
        Task SendLog(LogModel log);
    }
}
