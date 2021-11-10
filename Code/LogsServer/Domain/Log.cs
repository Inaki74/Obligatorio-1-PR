using System;

namespace Domain
{
    public class Log
    {
        public int Id {get;set;}
        public string Username {get; set;}
        public string Gamename {get;set;}
        public string Description {get;set;}
        public DateTime Date {get;set;}

        public override bool Equals(object obj)
        {
            Log objLog = (Log)obj;
            return objLog.Id == Id;
        }
    }
}
