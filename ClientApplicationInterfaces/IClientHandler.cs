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
        VaporStatusResponse PublishGame(GameNetworkTransferObject game);

        // lista de strings
        List<string> GetGames();

        // string
        VaporStatusResponse Login(UserNetworkTransferObject user);

        // string
        VaporStatusResponse Exit();
    }
}
