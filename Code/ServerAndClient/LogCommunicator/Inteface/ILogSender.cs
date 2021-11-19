using System.Threading.Tasks;
using Models;

namespace LogCommunicator.Interfaces
{
    public interface ILogSender
    {
        Task SendLog(LogModel log);
    }
}