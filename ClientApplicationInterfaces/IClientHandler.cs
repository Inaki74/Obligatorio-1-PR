using System;
using Common.Protocol;
using Common.Protocol.NTOs;

namespace ClientApplicationInterfaces
{
    public interface IClientHandler
    {
        static IClientHandler Instance { get; set; }

        bool ConnectToServer();

        VaporStatusResponse PublishGame(GameNetworkTransferObject game);
        VaporStatusResponse Login(UserNetworkTransferObject user);
        VaporStatusResponse Exit();
    }
}
