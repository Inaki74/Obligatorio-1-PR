using System;
using System.Collections.Generic;
using BusinessLogicInterfaces;
using DataAccess.Interface;
using ServerDomain;
using System.Linq;
using Models;

namespace BusinessLogic
{
    public class LogLogic : ILogLogic
    {
        private readonly ILogDataAccess<LogError> _errorDataAccess;
        private readonly ILogDataAccess<LogInfo> _infoDataAccess;
        public LogLogic(ILogDataAccess<LogError> errorDataAccess, ILogDataAccess<LogInfo> infoDataAccess)
        {
            _errorDataAccess = errorDataAccess;
            _infoDataAccess = infoDataAccess;
        }

        public bool Add(LogModel log)
        {
            Console.WriteLine($"ADDING: {log.Date}, {log.Description}, {log.Username}, {log.Gamename}");

            if(log.LogType == LogType.INFO)
            {
                return _infoDataAccess.Add(ToLog(log));
            }

            return _errorDataAccess.Add(ToLog(log));
        }

        public List<LogModel> Get(string username = "", string gamename = "", DateTime? date = null)
        {
            List<Log> listToFilter = GetLogs(username, gamename);
            List<LogModel> ret = new List<LogModel>();

            if(date != null)
            {
                // desde
                listToFilter = listToFilter.Where(log => log.Date >= date).ToList();
            }

            listToFilter.ForEach(l => ret.Add(new LogModel(l)));

            ret = ret.OrderBy(l => l.Id).ToList();

            return ret;
        }

        private List<Log> GetLogs(string username, string gamename)
        {
            if(String.IsNullOrEmpty(username) && String.IsNullOrEmpty(gamename))
            {
                return _errorDataAccess.GetAll().Union(_infoDataAccess.GetAll()).ToList();
            }

            if(!String.IsNullOrEmpty(username) && String.IsNullOrEmpty(gamename))
            {
                return _errorDataAccess.Get(username).Union(_infoDataAccess.Get(username)).ToList();
            }

            if(String.IsNullOrEmpty(username) && !String.IsNullOrEmpty(gamename))
            {
                return _errorDataAccess.Get("", gamename).Union(_infoDataAccess.Get("", gamename)).ToList();
            }

            if(!String.IsNullOrEmpty(username) && !String.IsNullOrEmpty(gamename))
            {
                return _errorDataAccess.Get(username, gamename).Union(_infoDataAccess.Get(username, gamename)).ToList();
            }

            return null;
        }

        private Log ToLog(LogModel model)
        {
            Log ret;

            if(model.LogType == LogType.INFO)
            {
                ret = new LogInfo();
            }
            else
            {
                ret = new LogError();
            }

            ret.Date = model.Date;
            ret.Description = model.Description;
            ret.Gamename = model.Gamename;
            ret.Id = model.Id;
            ret.Username = model.Username;

            return ret;
        }
    }
}