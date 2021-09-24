using System;

namespace Domain.BusinessObjects
{
    public class Review
    {
        public int ID {get; set;}
        public User ReviewPublisher {get; set;}
        public Game Game {get; set;}
        public int Score {get; set;}
        public string Description {get; set;}

        public Review()
        {

        }

        public override bool Equals(object obj)
        {
            Review rev = (Review)obj;

            return this.ReviewPublisher.Equals(rev.ReviewPublisher) && this.Game.Equals(rev.Game);
        }
    }
}
