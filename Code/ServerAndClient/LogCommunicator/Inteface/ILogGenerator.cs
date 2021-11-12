using Models;

namespace LogCommunicator.Interfaces
{
    public interface ILogGenerator
    {
        LogModel CreateLog(string username, int gameid, bool isError, string message);
    }
}