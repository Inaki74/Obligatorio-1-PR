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
        public void AddGame(Game game)
        {
            bool exists = GetAllGames().Exists(g => game.Equals(g));
            if (!exists)
            {
                User realOwner = _userDataAccess.Get(game.Owner.Username);
                game.Owner = realOwner;
                game.Id = LocalGameDataAccess.CurrentId;
                _gameDataAccess.Add(game);
                return;
            }
            
            throw new Exception("Game already exists!");
        }

        public void ModifyGame(Game game)
        {
            Game oldGame = _gameDataAccess.GetCopyId(game.Id);
            if (oldGame != null)
            {
                if (oldGame.Owner.Equals(game.Owner))
                {
                    Game finalGame = GetFinalGame(game, oldGame);
                    _gameDataAccess.Update(finalGame);
                    return; 
                }
                else
                {
                    throw new Exception("You dont have permission to modify this game");
                }
                
            }

            throw new Exception("Game doesnt exist!");
        }

        public List<Game> GetAllGames()
        {
            return _gameDataAccess.GetAll();
        }
        public Game SelectGame(string game)
        {
            Game dummyGame = new Game();
            dummyGame.Title = game;
            return GetAllGames().FirstOrDefault(g => g.Equals(dummyGame));
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
            dummyGame.Title = query.Gamename;
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
            Game game = _gameDataAccess.GetCopy(query.Gamename);

            return game.Owner.Username == query.Username;
        }

        public void DeleteGame(Game game)
        {
            // Remove for all users who acquired the game as well.
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
            if (modifiedGame.Title != "")
            {
                finalGame.Title = modifiedGame.Title;
            }

            if (modifiedGame.Genre != "")
            {
                finalGame.Genre = modifiedGame.Genre;
            }

            if (modifiedGame.ESRB != "")
            {
                finalGame.ESRB = modifiedGame.ESRB;
            }

            if (modifiedGame.Synopsis != "")
            {
                finalGame.Synopsis = modifiedGame.Synopsis;
            }
            
            return finalGame;
        }
    }
}