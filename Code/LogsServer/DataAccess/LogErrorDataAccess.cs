using System.Collections.Generic;
using DataAccess.Interface;
using Database.Interface;
using Domain;

namespace DataAccess
{
    public class LogErrorDataAccess : ILogDataAccess
    {
        private readonly IDatabase _database;
        public LogErrorDataAccess(IDatabase database)
        {
            _database = database;
        }

        public List<Log> Get(string username)
        {
            return LogDataAccessHelper.GetLogsFromDictionary(_database.ErrorLogs[username]);
        }

        public List<Log> Get(string username, string gamename)
        {
            if(string.IsNullOrEmpty(username))
            {
                return LogDataAccessHelper.GetGameLogsFromConcurrentDictionary(_database.ErrorLogs, gamename);
            }

            return _database.ErrorLogs[username][gamename];
        }

        public List<Log> GetAll()
        {
            return LogDataAccessHelper.GetUserLogsFromConcurrentDictionary(_database.ErrorLogs);
        }
    }
}