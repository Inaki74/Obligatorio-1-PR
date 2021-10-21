using System;
using System.Threading.Tasks;

namespace ServerApplicationInterfaces
{
    public interface IServerHandler
    {
        static IServerHandler Instance { get; set; }

        bool StartServer();

        Task StartClientListeningTask();

        void CloseServer();
    }
}
