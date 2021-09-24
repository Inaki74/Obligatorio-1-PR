using System.Linq;
using System.Collections.Generic;
using BusinessInterfaces;
using DataAccess;
using Domain.BusinessObjects;

namespace Business
{
    public class ReviewLogic : IReviewLogic
    {
        private IDataAccess<Game> _gameDataAccess = new LocalGameDataAccess();
        private IDataAccess<User> _userDataAccess = new LocalUserDataAccess();
        private IDataAccess<Review> _reviewDataAccess = new LocalReviewDataAccess();

        public void AddReview(Review review)
        {
            User actualUser = _userDataAccess.Get(review.ReviewPublisher.Username);
            Game actualGame = _gameDataAccess.Get(review.Game.Title);
            review.ReviewPublisher = actualUser;
            review.Game = actualGame;

            _reviewDataAccess.Add(review);
        }

        public bool Exists(Review review)
        {
            List<Review> allReviews = _reviewDataAccess.GetAll();

            return allReviews.Exists(r => r.Equals(review));
        }
    }
}
