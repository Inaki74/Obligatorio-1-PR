using System;

namespace Domain
{
    public class User
    {
        public string Username { get; set; }
        public bool LoggedIn { get; set; }
        public int ID {get; set;}

        public User(string username, int id)
        {
            this.Username = username;
            this.LoggedIn = false;
            this.ID = id;
        }
    }
}