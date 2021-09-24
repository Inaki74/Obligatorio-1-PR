using System;
using System.Collections.Generic;
using Domain.BusinessObjects;

namespace BusinessInterfaces
{
    public interface IReviewLogic
    {
        void AddReview(Review review);

        bool Exists(Review review);
        List<Review> GetReviews(Game game);
        float GetGameScore(Game game);
        float GetGameScore(List<Review> reviewList);
    }
}
