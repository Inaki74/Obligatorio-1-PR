using System;
using Models;

namespace LogCommunicatorInterfaces
{
    public interface ILogGenerator
    {
        LogModel CreateLog(string username, int gameid, bool isError, string message);
    }
}
