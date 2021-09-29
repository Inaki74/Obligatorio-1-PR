using System;
using System.Collections.Generic;
using System.Linq;
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
        public int AddGame(Game game)
        {
            bool exists = GetAllGames().Exists(g => game.Equals(g));
            if (!exists)
            {
                User realOwner = _userDataAccess.Get(game.Owner.Username);
                game.Owner = realOwner;
                game.Id = LocalGameDataAccess.CurrentId;
                _gameDataAccess.Add(game);
                return game.Id;
            }
            
            throw new Exception("Game already exists!");
        }

        public void ModifyGame(Game game)
        {
            Game oldGame = _gameDataAccess.GetCopyId(game.Id);
            Game finalGame = GetFinalGame(game, oldGame);
            
            List<User> userList = _userDataAccess.GetAll();
            foreach (User user in userList)
            {
                if (HasGame(user, oldGame))
                {
                    user.ownedGames.Remove(oldGame);
                    user.ownedGames.Add(finalGame);
                    _userDataAccess.Update(user);
                }
            }
            
            _gameDataAccess.Update(finalGame);
        }

        public Game GetGame(int id)
        {
            return _gameDataAccess.GetCopyId(id);
        }

        public List<Game> GetAllGames()
        {
            return _gameDataAccess.GetAll();
        }
        public Game SelectGame(int id)
        {
            Game dummyGame = new Game();
            dummyGame.Title = "";
            dummyGame.Id = id;
            return GetAllGames().FirstOrDefault(g => g.Equals(dummyGame));
        }

        public int GetGameId(string title)
        {
            Game dummyGame = new Game();
            dummyGame.Title = title;
            Game found = GetAllGames().FirstOrDefault(g => g.Equals(dummyGame));
            return found.Id;
        }
        
        public List<Game> SearchGames(GameSearchQuery query)
        {
            List<Game> allGames = _gameDataAccess.GetAll();

            return FilterGames(allGames, query);
        }

        public bool AcquireGame(GameUserRelationQuery query)
        {
            //TODO: Is this thread safe? Check.
            Game dummyGame = new Game();
            dummyGame.Id = query.Gameid;
            Game realGame = GetAllGames().FirstOrDefault(g => g.Equals(dummyGame));
            bool gameAcquired = false;
            if (realGame != null)
            {
                User user = _userDataAccess.Get(query.Username);
                user.ownedGames.Add(realGame);
                _userDataAccess.Update(user);
                gameAcquired = true;
            }

            return gameAcquired;
        }

        public bool CheckIsOwner(GameUserRelationQuery query)
        {
            Game game = _gameDataAccess.GetCopyId(query.Gameid);

            return game.Owner.Username == query.Username;
        }

        public void DeleteGame(Game game)
        {
            List<User> userList = _userDataAccess.GetAll();
            foreach (User user in userList)
            {
                if (HasGame(user, game))
                {
                    user.ownedGames.Remove(game);
                    _userDataAccess.Update(user);
                }
            }
            _gameDataAccess.Delete(game);
        }

        private List<Game> FilterGames(List<Game> games, GameSearchQuery query)
        {
            List<Game> filteredGames = new List<Game>();
            foreach(Game game in games)
            {
                if(FulfillsQuery(game, query))
                {
                    filteredGames.Add(game);
                }
            }
            return filteredGames;
        }

        private bool FulfillsQuery(Game game, GameSearchQuery query)
        {
            bool titleCoincidence = game.FulfillsTitle(query.Title);
            bool genreCoincidence = game.FulfillsGenre(query.Genre);
            bool scoreCoincidence = FulfillsScoreQuery(game, query);

            return titleCoincidence && genreCoincidence && scoreCoincidence;
        }

        private bool FulfillsScoreQuery(Game game, GameSearchQuery query)
        {
            IReviewLogic reviewLogic = new ReviewLogic();
            float score = reviewLogic.GetGameScore(game);

            if(query.Score == 0) return true;

            return score >= query.Score;
        }

        private Game GetFinalGame(Game modifiedGame, Game oldGame)
        {
            Game finalGame = _gameDataAccess.GetCopyId(oldGame.Id);

            finalGame.Title = modifiedGame.Title != "" ? modifiedGame.Title : finalGame.Title;
            finalGame.Genre = modifiedGame.Genre != "" ? modifiedGame.Genre : finalGame.Genre;
            finalGame.ESRB = modifiedGame.ESRB != "" ? modifiedGame.ESRB : finalGame.ESRB;
            finalGame.Synopsis = modifiedGame.Synopsis != "" ? modifiedGame.Synopsis : finalGame.Synopsis;
            
            return finalGame;
        }

        private bool HasGame(User user, Game game)
        {
            foreach (Game currentGame in user.ownedGames)
            {
                if (currentGame.Equals(game)) return true;
            }

            return false;
        }
    }
}