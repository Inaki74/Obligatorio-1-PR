using System;
using Domain.HelperObjects;

namespace Domain.BusinessObjects
{
    [Serializable]
    public class Game
    {
        public User Owner { get; set; }
        public string Title { get; set; }
        public string Genre {get; set;}
        public string ESRB {get; set;}
        public string Synopsis {get; set;}
        public string CoverPath {get; set;}

        public Game(){}
        public Game(string title, string genre, string esrb, string synopsis, string path)
        {
            //Owner = user;
            Title = title;
            Genre = genre;
            ESRB = esrb;
            Synopsis = synopsis;
            CoverPath = path;
        }

        public override bool Equals(object obj)
        {
            Game gameInObj = (Game)obj;

            string gameInObjTitleLower = gameInObj.Title.ToLower();
            string thisTitleLower = this.Title.ToLower();

            return gameInObjTitleLower == thisTitleLower;
        }
        public bool FulfillsTitle(string aTitle)
        {
            if(string.IsNullOrEmpty(aTitle))
            {
                return true;
            }

            string noCapsInTitle = aTitle.ToLower();
            string noCapsTitle = Title.ToLower();

            return noCapsTitle.Contains(noCapsInTitle);
        }

        public bool FulfillsGenre(string aGenre)
        {
            if(string.IsNullOrEmpty(aGenre))
            {
                return true;
            }

            string noCapsInGenre = aGenre.ToLower();
            string noCapsGenre = Genre.ToLower();

            return noCapsGenre.Equals(noCapsInGenre);
        }
    }
}