using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Protocol;
using Common.Protocol.NTOs;

namespace ClientApplicationInterfaces
{
    public interface IClientHandler
    {
        static IClientHandler Instance { get; set; }

        bool ConnectToServer();
        Task<VaporStatusResponse> PublishGameAsync(GameNetworkTransferObject game);

        string DeleteGame();

        Task<string> ModifyGame(GameNetworkTransferObject game);

        string PublishReview(ReviewNetworkTransferObject game);

        VaporStatusResponse GetGames();

        VaporStatusResponse GetGameScore();
        
        VaporStatusResponse GetGameReview(string username);

        VaporStatusResponse GetGameDetails();

        Task<VaporStatusResponse> DownloadGameCover(string path);

        VaporStatusResponse SearchGames(GameSearchQueryNetworkTransferObject query);

        VaporStatusResponse CheckIsOwner();

        VaporStatusResponse Login(UserNetworkTransferObject user);

        VaporStatusResponse SelectGame(string game);
        
        VaporStatusResponse AcquireGame();

        Task<string> ExitAsync();
    }
}
