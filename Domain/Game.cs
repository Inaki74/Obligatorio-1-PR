using System;

namespace Domain
{
    public class Game
    {
        public User Owner { get; set; }
        public string Title { get; set; }
        public string Genre {get; set;}
        public string ESRB {get; set;}
        public string Synopsis {get; set;}
        public string CoverPath {get; set;}
        public int OverallScore {get; set;}

        public Game(){}
        public Game(string title, string genre, string esrb, string synopsis, string path)
        {
            //Owner = user;
            Title = title;
            Genre = genre;
            ESRB = esrb;
            Synopsis = synopsis;
            CoverPath = path;
            OverallScore = 0;
        }

        public bool FulfillsQuery(GameSearchQuery query)
        {
            return FulfillsTitle(query.Title) || FulfillsGenre(query.Genre) || FulfillsScore(query.Score);
        }

        private bool FulfillsTitle(string aTitle)
        {
            return Title.Contains(aTitle);
        }

        private bool FulfillsGenre(string aGenre)
        {
            if(string.IsNullOrEmpty(aGenre))
            {
                return false;
            }

            return Genre.Equals(aGenre);
        }

        private bool FulfillsScore(int aScore)
        {
            if(aScore == 0)
            {
                return false;
            }

            return OverallScore >= aScore;
        }
    }
}