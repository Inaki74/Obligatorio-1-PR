using System;
using System.Collections.Generic;
using System.Linq;
using BusinessInterfaces;
using DataAccess;
using Domain.BusinessObjects;
using Domain.HelperObjects;
using Exceptions.BusinessExceptions;

namespace Business
{
    public class GameLogic : IGameLogic
    {
        private IDataAccess<Game> _gameDataAccess = new LocalGameDataAccess();
        private IDataAccess<User> _userDataAccess = new LocalUserDataAccess();
        public int AddGame(Game game)
        {
            if(string.IsNullOrEmpty(game.Title))
            {
                throw new GameKeyFormatException();
            }

            bool exists = GetAllGames().Exists(g => game.Equals(g));
            if (!exists)
            {
                string username = game.Owner.Username;
                User realOwner = _userDataAccess.Get(username);
                game.Owner = realOwner;
                game.Id = LocalGameDataAccess.CurrentId;
                _gameDataAccess.Add(game);
                return game.Id;
            }
            
            throw new GameAlreadyExistsException();
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
            
            IReviewLogic reviewLogic = new ReviewLogic();
            List<Review> gameReviews = reviewLogic.GetReviews(oldGame);
            foreach (Review review in gameReviews)
            {
                review.Game = finalGame;
            }
            reviewLogic.BulkUpdate(gameReviews);
            
            _gameDataAccess.Update(finalGame);
        }

        public Game GetGame(int id)
        {
            return _gameDataAccess.GetCopyId(id);
        }

        public Game GetGame(string title)
        {
            return _gameDataAccess.GetCopy(title);
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
            
            try
            {
                Game found = GetAllGames().First(g => g.Equals(dummyGame));
                return found;
            }
            catch(ArgumentNullException ane)
            {
                throw new FindGameException(ane.Message);
            }
            catch(InvalidOperationException ioe)
            {
                throw new FindGameException(ioe.Message);
            }
        }

        public int GetGameId(string title)
        {
            Game dummyGame = new Game();
            dummyGame.Title = title;

            try
            {
                Game found = GetAllGames().First(g => g.Equals(dummyGame));
                return found.Id;
            }
            catch(ArgumentNullException ane)
            {
                throw new FindGameException(ane.Message);
            }
            catch(InvalidOperationException ioe)
            {
                throw new FindGameException(ioe.Message);
            }
        }
        
        public List<Game> SearchGames(GameSearchQuery query)
        {
            List<Game> allGames = _gameDataAccess.GetAll();

            return FilterGames(allGames, query);
        }

        public void AcquireGame(GameUserRelationQuery query)
        {
            Game dummyGame = new Game();
            dummyGame.Id = query.Gameid;

            try
            {
                Game realGame = GetAllGames().First(g => g.Equals(dummyGame));
                User user = _userDataAccess.Get(query.Username);

                if(user.ownedGames.Contains(realGame))
                {
                    throw new UserAlreadyAcquiredGame(realGame.Title);
                }

                user.ownedGames.Add(realGame);
                _userDataAccess.Update(user);
            }
            catch(ArgumentNullException ane)
            {
                throw new FindGameException(ane.Message);
            }
            catch(InvalidOperationException ioe)
            {
                throw new FindGameException(ioe.Message);
            }
        }

        public void UnacquireGame(GameUserRelationQuery query)
        {
            Game dummyGame = new Game();
            dummyGame.Id = query.Gameid;

            try
            {
                Game realGame = GetAllGames().First(g => g.Equals(dummyGame));
                User user = _userDataAccess.Get(query.Username);

                if(!user.ownedGames.Contains(realGame))
                {
                    throw new UserDidntAcquireGame(realGame.Title);
                }

                user.ownedGames.Remove(realGame);
                _userDataAccess.Update(user);
            }
            catch(ArgumentNullException ane)
            {
                throw new FindGameException(ane.Message);
            }
            catch(InvalidOperationException ioe)
            {
                throw new FindGameException(ioe.Message);
            }
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