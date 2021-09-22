using System;
using System.Collections.Generic;
using Domain;

namespace BusinessInterfaces
{
    public interface IGameLogic
    {
        void AddGame(Game game);

        List<Game> GetAllGames();

        bool SelectGame(string game);

        List<Game> SearchGames(GameSearchQuery query);

    }
}
