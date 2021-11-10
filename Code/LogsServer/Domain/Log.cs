using System;

namespace Domain
{
    public class Log
    {
        int Id {get;set;}
        LogType LogType{get;set;}
        string Username {get; set;}
        string Gamename {get;set;}
        string Description {get;set;}
        DateTime Date {get;set;}

        public override bool Equals(object obj)
        {
            Log objLog = (Log)obj;
            return objLog.Id == Id;
        }
    }
}
