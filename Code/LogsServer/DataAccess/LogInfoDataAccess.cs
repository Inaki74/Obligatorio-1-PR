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
            string gamename = NAME_IF_GAMENAME_EMPTY;

            if(!string.IsNullOrEmpty(log.Gamename))
            {
                gamename = log.Gamename;
            }

            return LogDataAccessHelper.AddLogsToDictionary(_database.InfoLogs, gamename, log);
        }

        public List<Log> Get(string username)
        {
            if(!_database.InfoLogs.ContainsKey(username))
            {
                return new List<Log>();
            }

            return LogDataAccessHelper.GetLogsFromDictionary(_database.InfoLogs[username]);
        }

        public List<Log> Get(string username, string gamename)
        {
            if(string.IsNullOrEmpty(username) || !_database.InfoLogs.ContainsKey(username))
            {
                return LogDataAccessHelper.GetGameLogsFromConcurrentDictionary(_database.InfoLogs, gamename);
            }

            if(!_database.InfoLogs[username].ContainsKey(username))
            {
                return new List<Log>();
            }

            return _database.InfoLogs[username][gamename];
        }

        public List<Log> GetAll()
        {
            return LogDataAccessHelper.GetUserLogsFromConcurrentDictionary(_database.InfoLogs);
        }
    }
}