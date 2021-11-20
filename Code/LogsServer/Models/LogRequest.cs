using System;

namespace Models
{
    public class LogRequest
    {
        public LogRequest() {}

        public LogRequest(string username, string gamename, DateTime date)
        {
            Username = username;
            Gamename = gamename;
            Date = date;
        }

        public string Username {get; set;}
        public string Gamename {get; set;}
        public DateTime Date {get;set;}
    }
}