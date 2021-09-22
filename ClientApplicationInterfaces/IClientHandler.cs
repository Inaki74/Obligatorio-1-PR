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

        //TODO: Cambiar a cada cosa necesaria

        // string
        string PublishGame(GameNetworkTransferObject game);

        // lista de strings
        List<string> GetGames();

        // string
        VaporStatusResponse<string> Login(UserNetworkTransferObject user);

        // string
        string Exit();
    }
}
