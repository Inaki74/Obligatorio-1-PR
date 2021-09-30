using System.Collections.Generic;
using Domain.BusinessObjects;

namespace Domain.HelperObjects
{
    public class DetailsQuery
    {
        public List<Review> Reviews { get; set; }
        public Game Game { get; set; }
        public float Score { get; set; }
    }
}