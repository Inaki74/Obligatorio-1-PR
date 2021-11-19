using System;

namespace Models
{
    public class LogModel
    {
        public int Id {get;set;}
        public LogType LogType {get;set;}
        public string Username {get; set;}
        public string Gamename {get;set;}
        public string Description {get;set;}
        public DateTime Date {get;set;}
    }
}
