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
    }
}