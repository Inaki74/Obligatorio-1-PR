using System;

namespace Domain.HelperObjects
{
    public class GameOwnershipQuery
    {
        public string Username { get; set; }
        public string Gamename {get; set;}
        public GameOwnershipQuery(){}
        public GameOwnershipQuery(string username, string gamename)
        {
            Username = username;
            Gamename = gamename;
        }
    }
}