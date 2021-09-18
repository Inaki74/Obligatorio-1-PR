using System;
using Common.Protocol;

namespace ClientApplicationInterfaces
{
    public interface IClientHandler
    {
        static IClientHandler Instance { get; set; }

        bool ConnectToServer();

        VaporStatusMessage Login(string username);

        void Exit();
    }
}
