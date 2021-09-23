using System;
using Domain.HelperObjects;

namespace Domain.BusinessObjects
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

        public override bool Equals(object obj)
        {
            Game gameInObj = (Game)obj;

            string gameInObjTitleLower = gameInObj.Title.ToLower();
            string thisTitleLower = this.Title.ToLower();

            return gameInObjTitleLower == thisTitleLower;
        }

        private bool FulfillsTitle(string aTitle)
        {
            if(string.IsNullOrEmpty(aTitle))
            {
                return false;
            }

            string noCapsInTitle = aTitle.ToLower();
            string noCapsTitle = Title.ToLower();

            return noCapsTitle.Contains(noCapsInTitle);
        }

        private bool FulfillsGenre(string aGenre)
        {
            if(string.IsNullOrEmpty(aGenre))
            {
                return false;
            }

            string noCapsInGenre = aGenre.ToLower();
            string noCapsGenre = Genre.ToLower();

            return noCapsGenre.Equals(noCapsInGenre);
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