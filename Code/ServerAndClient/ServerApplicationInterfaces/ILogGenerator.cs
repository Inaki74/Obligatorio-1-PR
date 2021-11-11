using Models;

namespace ServerApplicationInterfaces
{
    public interface ILogGenerator
    {
        LogModel CreateLog(string username, string gamename, int statuscode, string message);
    }
}