using System.Collections.Generic;
using DataAccess.Interface;
using Database.Interface;
using ServerDomain;

namespace DataAccess
{
    public class LogInfoDataAccess : ILogDataAccess<LogInfo>
    {
        private const string NAME_IF_GAMENAME_EMPTY = "GameEmpty";
        private readonly IDatabase _database;
        public LogInfoDataAccess(IDatabase database)
        {
            _database = database;
        }

        public bool Add(Log log)
        {
            return LogDataAccessHelper.AddLogsToDictionary(_database.InfoLogs, NAME_IF_GAMENAME_EMPTY, log);
        }

        public List<Log> Get(string username)
        {
            return LogDataAccessHelper.GetLogsFromDictionary(_database.InfoLogs[username]);
        }

        public List<Log> Get(string username, string gamename)
        {
            if(string.IsNullOrEmpty(username))
            {
                return LogDataAccessHelper.GetGameLogsFromConcurrentDictionary(_database.ErrorLogs, gamename);
            }

            return _database.InfoLogs[username][gamename];
        }

        public List<Log> GetAll()
        {
            return LogDataAccessHelper.GetUserLogsFromConcurrentDictionary(_database.InfoLogs);
        }
    }
}