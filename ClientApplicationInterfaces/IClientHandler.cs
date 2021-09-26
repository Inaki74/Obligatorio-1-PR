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

        string DeleteGame();

        string ModifyGame(GameNetworkTransferObject game);

        string PublishReview(ReviewNetworkTransferObject game);

        VaporStatusResponse GetGames();

        VaporStatusResponse GetGameScore();

        VaporStatusResponse SearchGames(GameSearchQueryNetworkTransferObject query);

        VaporStatusResponse CheckIsOwner();

        VaporStatusResponse Login(UserNetworkTransferObject user);

        VaporStatusResponse SelectGame(string game);
        
        VaporStatusResponse AcquireGame();

        string Exit();
    }
}
