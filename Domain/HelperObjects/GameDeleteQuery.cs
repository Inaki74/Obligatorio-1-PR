using System;

namespace Domain.HelperObjects
{
    public class GameDeleteQuery
    {
        public string Username { get; set; }
        public string Gamename {get; set;}
        public GameDeleteQuery(){}
        public GameDeleteQuery(string username, string gamename)
        {
            Username = username;
            Gamename = gamename;
        }
    }
}
