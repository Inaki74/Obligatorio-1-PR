using System;
using System.Collections.Generic;
using Domain.HelperObjects;
using Domain.BusinessObjects;

namespace BusinessInterfaces
{
    public interface IGameLogic
    {
        int AddGame(Game game);

        void ModifyGame(Game game);
        Game GetGame(int id);
        Game GetGame(string title);

        List<Game> GetAllGames();

        Game SelectGame(int id);
        int GetGameId(string title);

        List<Game> SearchGames(GameSearchQuery query);

        void AcquireGame(GameUserRelationQuery query);
        
        bool CheckIsOwner(GameUserRelationQuery query);

        void DeleteGame(Game game);

    }
}
