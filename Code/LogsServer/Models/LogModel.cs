using System;
using ServerDomain;

namespace Models
{
    public class LogModel
    {
        public LogModel()
        {

        }

        public LogModel(Log log)
        {
            this.Id = log.Id;
            this.Username = log.Username;
            this.Gamename = log.Gamename;
            this.Description = log.Description;
            this.Date = log.Date;

            if(log.GetType().Equals(typeof(LogError)))
            {
                this.LogType = LogType.ERROR;
            }
            if(log.GetType().Equals(typeof(LogInfo)))
            {
                this.LogType = LogType.INFO;
            }
        }

        public int Id {get;set;}
        public LogType LogType {get;set;}
        public string Username {get; set;}
        public string Gamename {get;set;}
        public string Description {get;set;}
        public DateTime Date {get;set;}
    }
}
