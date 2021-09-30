using System;
using System.Collections.Generic;
using Domain.BusinessObjects;
using Domain.HelperObjects;

namespace BusinessInterfaces
{
    public interface IReviewLogic
    {
        void AddReview(Review review);

        bool Exists(Review review);
        List<Review> GetReviews(Game game);
        float GetGameScore(Game game);
        float GetGameScore(List<Review> reviewList);
        Review GetReview(GameUserRelationQuery reviewDummy);
        void BulkUpdate(List<Review> reviewList);
        
    }
}
