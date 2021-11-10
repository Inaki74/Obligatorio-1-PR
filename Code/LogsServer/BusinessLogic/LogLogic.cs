using System;
using System.Collections.Generic;
using BusinessLogicInterfaces;
using DataAccess.Interface;
using Domain;
using System.Linq;
namespace BusinessLogic
{
    public class LogLogic : ILogLogic
    {
        private readonly ILogDataAccess _dataAccess;
        public LogLogic(ILogDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public bool Add(Log log)
        {
            //return _dataAccess.Add(log);
        }

        public List<Log> Get(string username = "", string gamename = "", DateTime? date = null)
        {
            List<Log> listToFilter = GetLogs(username, gamename);

            if(date != null)
            {
                // desde
                return listToFilter.Where(log => log.Date >= date).ToList();
            }

            return listToFilter;
        }

        private List<Log> GetLogs(string username, string gamename)
        {
            if(String.IsNullOrEmpty(username) && String.IsNullOrEmpty(gamename))
            {
                return _dataAccess.GetAll();
            }

            if(!String.IsNullOrEmpty(username) && String.IsNullOrEmpty(gamename))
            {
                return _dataAccess.Get(username);
            }

            if(String.IsNullOrEmpty(username) && !String.IsNullOrEmpty(gamename))
            {
                return _dataAccess.Get("", gamename);
            }

            if(!String.IsNullOrEmpty(username) && !String.IsNullOrEmpty(gamename))
            {
                return _dataAccess.Get(username, gamename);
            }

            return null;
        }
    }
}