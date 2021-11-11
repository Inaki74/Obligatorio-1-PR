using System;
using Common.Protocol;
using Models;
using ServerApplicationInterfaces;

namespace ServerApplication
{
    public class LogsGenerator : ILogGenerator
    {
        public LogModel CreateLog(string username, string gamename, int statuscode, string message)
        {
            LogModel model = new LogModel();

            model.Date = DateTime.Now;
            model.Username = username;
            model.Gamename = gamename;
            model.Description = message;

            if(statuscode == StatusCodeConstants.INFO || statuscode == StatusCodeConstants.OK)
            {
                model.LogType = LogType.INFO;
            }
            else
            {
                model.LogType = LogType.ERROR;
            }

            return model;
        }
    }
}