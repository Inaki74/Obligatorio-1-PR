using System;

namespace Domain
{
    public class Log
    {
        LogType LogType{get;set;}
        string Username {get; set;}
        string Gamename {get;set;}
        string Description {get;set;}
        DateTime Date {get;set;}
    }
}
