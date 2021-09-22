using System;
using System.Collections.Generic;
using Common.Protocol;
using Common.Protocol.NTOs;

namespace ClientApplicationInterfaces
{
    public interface IClientHandler
    {
        static IClientHandler Instance { get; set; }

        bool ConnectToServer();
        string PublishGame(GameNetworkTransferObject game);

        VaporStatusResponse GetGames();

        VaporStatusResponse SearchGames(GameSearchQueryNetworkTransferObject query);

        VaporStatusResponse Login(UserNetworkTransferObject user);

        string Exit();
    }
}
