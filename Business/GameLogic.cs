using System;
using BusinessInterfaces;
using DataAccess;
using Domain;

namespace Business
{
    public class GameLogic : IGameLogic
    {
        private IDataAccess<Game> _userDataAccess = new LocalGameDataAccess();

        public void AddGame(Game game)
        {
            _userDataAccess.Add(game);
        }
    }
}