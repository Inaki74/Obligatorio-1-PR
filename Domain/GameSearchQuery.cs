using System;

namespace Domain
{
    public class GameSearchQuery
    {
        public string Title { get; set; }
        public string Genre {get; set;}
        public int Score {get; set;}

        public GameSearchQuery(){}
        public GameSearchQuery(string title, string genre, int score)
        {
            Title = title;
            Genre = genre;
            Score = score;
        }
    }
}
