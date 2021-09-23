using System;

namespace Domain.HelperObjects
{
    public class GameUserRelationQuery
    {
        public string Username { get; set; }
        public string Gamename {get; set;}
        public GameUserRelationQuery(){}
        public GameUserRelationQuery(string username, string gamename)
        {
            Username = username;
            Gamename = gamename;
        }
    }
}