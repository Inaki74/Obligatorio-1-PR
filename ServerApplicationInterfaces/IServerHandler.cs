using System;

namespace ServerApplicationInterfaces
{
    public interface IServerHandler
    {
        static IServerHandler Instance { get; set; }

        bool StartServer();

        void StartClientListeningThread();

        void StartClientThread();
    }
}
