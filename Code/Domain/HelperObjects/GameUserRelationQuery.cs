using System;

namespace Domain.HelperObjects
{
    public class GameUserRelationQuery
    {
        public string Username { get; set; }
        public int Gameid {get; set;}
        public GameUserRelationQuery(){}
        public GameUserRelationQuery(string username, int gameid)
        {
            Username = username;
            Gameid = gameid;
        }
    }
}