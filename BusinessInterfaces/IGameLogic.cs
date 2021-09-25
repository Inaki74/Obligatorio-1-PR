using System;
using System.Collections.Generic;
using Domain.HelperObjects;
using Domain.BusinessObjects;

namespace BusinessInterfaces
{
    public interface IGameLogic
    {
        void AddGame(Game game);

        void ModifyGame(Game game);

        List<Game> GetAllGames();

        bool SelectGame(string game);

        List<Game> SearchGames(GameSearchQuery query);

        bool AcquireGame(GameUserRelationQuery query);
        
        bool CheckIsOwner(GameUserRelationQuery query);

        void DeleteGame(Game game);

    }
}
