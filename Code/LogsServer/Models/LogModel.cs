using System;
using Domain;

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

        public Log ToLog()
        {
            Log ret;

            if(LogType == LogType.INFO)
            {
                ret = new LogInfo();
            }
            else
            {
                ret = new LogError();
            }

            ret.Date = Date;
            ret.Description = Description;
            ret.Gamename = Gamename;
            ret.Id = Id;
            ret.Username = Username;

            return ret;
        }
    }
}
