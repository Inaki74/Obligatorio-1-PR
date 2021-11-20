using System;
using System.Collections.Generic;
using DataAccess.Interface;
using Database.Interface;
using ServerDomain;

namespace DataAccess
{
    public class LogErrorDataAccess : ILogDataAccess<LogError>
    {
        private const string NAME_IF_GAMENAME_EMPTY = "GameEmpty";
        private readonly IDatabase _database;
        public LogErrorDataAccess(IDatabase database)
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

            return LogDataAccessHelper.AddLogsToDictionary(_database.ErrorLogs, gamename, log);
        }

        public List<Log> Get(string username)
        {
            if(!_database.ErrorLogs.ContainsKey(username))
            {
                return new List<Log>();
            }

            return LogDataAccessHelper.GetLogsFromDictionary(_database.ErrorLogs[username]);
        }

        public List<Log> Get(string username, string gamename)
        {
            if(string.IsNullOrEmpty(username) || !_database.ErrorLogs.ContainsKey(username))
            {
                return LogDataAccessHelper.GetGameLogsFromConcurrentDictionary(_database.ErrorLogs, gamename);
            }

            if(!_database.ErrorLogs[username].ContainsKey(username))
            {
                return new List<Log>();
            }

            return _database.ErrorLogs[username][gamename];
        }

        public List<Log> GetAll()
        {
            return LogDataAccessHelper.GetUserLogsFromConcurrentDictionary(_database.ErrorLogs);
        }
    }
}