using System.Linq;
using System.Collections.Generic;
using BusinessInterfaces;
using DataAccess;
using Domain.BusinessObjects;
using Domain.HelperObjects;

namespace Business
{
    public class ReviewLogic : IReviewLogic
    {
        private IDataAccess<Game> _gameDataAccess = new LocalGameDataAccess();
        private IDataAccess<User> _userDataAccess = new LocalUserDataAccess();
        private IDataAccess<Review> _reviewDataAccess = new LocalReviewDataAccess();

        public void AddReview(Review review)
        {
            if(Exists(review))
            {
                _reviewDataAccess.Delete(review);
            }

            User actualUser = _userDataAccess.Get(review.ReviewPublisher.Username);
            Game actualGame = _gameDataAccess.Get(review.Game.Id);
            review.ReviewPublisher = actualUser;
            review.Game = actualGame;

            _reviewDataAccess.Add(review);
        }

        public bool Exists(Review review)
        {
            List<Review> allReviews = _reviewDataAccess.GetAll();

            return allReviews.Exists(r => r.Equals(review));
        }

        public float GetGameScore(Game game)
        {
            return GetGameScore(GetReviews(game));
        }

        public float GetGameScore(List<Review> reviews)
        {
            int score = 0;

            foreach(Review review in reviews)
            {
                score += review.Score;
            }

            float finalScore = (float)score / reviews.Count;

            return finalScore;
        }

        public Review GetReview(GameUserRelationQuery reviewDummy)
        {
            Game game = _gameDataAccess.Get(reviewDummy.Gameid);
            User owner = _userDataAccess.Get(reviewDummy.Username);
            List<Review> gameReviews = GetReviews(game);
            Review gameUserReview = gameReviews.FirstOrDefault(r => r.ReviewPublisher.Equals(owner));

            return gameUserReview;
        }

        public List<Review> GetReviews(Game game)
        {
            List<Review> allReviews = _reviewDataAccess.GetAll();
            List<Review> thisGamesReviews = new List<Review>();

            allReviews.ForEach(r => 
            {
                if(r.Game.Equals(game))
                {
                    thisGamesReviews.Add(r);
                }
            });

            return thisGamesReviews;
        }
    }
}
