using System;
using System.Threading.Tasks;

namespace ServerApplicationInterfaces
{
    public interface IServerHandler
    {
        static IServerHandler Instance { get; set; }

        Task<bool> StartServerAsync();

        Task StartClientListeningTaskAsync();

        void StartCloseServerTask();
    }
}
