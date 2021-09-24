using System;
using Domain.BusinessObjects;

namespace BusinessInterfaces
{
    public interface IReviewLogic
    {
        void AddReview(Review review);

        bool Exists(Review review);
    }
}
