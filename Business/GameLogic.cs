using System;
using System.Collections.Generic;
using BusinessInterfaces;
using DataAccess;
using Domain.BusinessObjects;
using Domain.HelperObjects;

namespace Business
{
    public class GameLogic : IGameLogic
    {
        private IDataAccess<Game> _gameDataAccess = new LocalGameDataAccess();
        private IDataAccess<User> _userDataAccess = new LocalUserDataAccess();
        public void AddGame(Game game)
        {
            bool exists = GetAllGames().Exists(g => game.Equals(g));
            if (!exists)
            {
                User realOwner = _userDataAccess.Get(game.Owner.Username);
                game.Owner = realOwner;
                _gameDataAccess.Add(game);
                return;
            }
            
            throw new Exception("Game already exists!");
        }

        public List<Game> GetAllGames()
        {
            return _gameDataAccess.GetAll();
        }
        

        public bool SelectGame(string game)
        {
            Game dummyGame = new Game();
            dummyGame.Title = game;
            return GetAllGames().Exists(g => g.Equals(dummyGame));
        }
        
        public List<Game> SearchGames(GameSearchQuery query)
        {
            List<Game> allGames = _gameDataAccess.GetAll();

            return FilterGames(allGames, query);
        }

        private List<Game> FilterGames(List<Game> games, GameSearchQuery query)
        {
            List<Game> filteredGames = new List<Game>();
            foreach(Game game in games)
            {
                if(game.FulfillsQuery(query))
                {
                    filteredGames.Add(game);
                }
            }
            return filteredGames;
    
        }
    }
}