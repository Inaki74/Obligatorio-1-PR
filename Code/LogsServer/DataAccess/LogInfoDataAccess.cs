using System.Collections.Generic;
using DataAccess.Interface;
using Database.Interface;
using Domain;

namespace DataAccess
{
    public class LogInfoDataAccess : ILogDataAccess
    {
        private readonly IDatabase _database;
        public LogInfoDataAccess(IDatabase database)
        {
            _database = database;
        }
        
        public List<Log> Get(string username)
        {
            return LogDataAccessHelper.GetLogsFromDictionary(_database.InfoLogs[username]);
        }

        public List<Log> Get(string username, string gamename)
        {
            return _database.InfoLogs[username][gamename];
        }

        public List<Log> GetAll()
        {
            return LogDataAccessHelper.GetLogsFromConcurrentDictionary(_database.InfoLogs);
        }
    }
}