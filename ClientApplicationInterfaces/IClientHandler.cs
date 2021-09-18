using System;
using Common.Protocol;

namespace ClientApplicationInterfaces
{
    public interface IClientHandler
    {
        static IClientHandler Instance { get; set; }

        bool ConnectToServer();

        VaporStatusResponse Login(string username);

        VaporStatusResponse Exit();
    }
}
