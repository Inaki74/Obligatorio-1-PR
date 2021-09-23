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

        string DeleteGame(GameNetworkTransferObject query);

        VaporStatusResponse GetGames();

        VaporStatusResponse SearchGames(GameSearchQueryNetworkTransferObject query);

        bool CheckIsOwner();

        VaporStatusResponse Login(UserNetworkTransferObject user);

        VaporStatusResponse SelectGame(string game);

        string Exit();
    }
}
