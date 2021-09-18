using System;
using Common.Protocol;

namespace ClientApplicationInterfaces
{
    public interface IClientHandler
    {
        static IClientHandler Instance { get; set; }

        bool ConnectToServer();

        VaporStatusResponse PublishGame(string game);
        VaporStatusResponse Login(string username);
        VaporStatusResponse Exit();
    }
}
