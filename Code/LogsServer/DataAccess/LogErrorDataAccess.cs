using System.Collections.Generic;
using DataAccess.Interface;
using Database.Interface;
using Domain;
using System.Linq;

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
            List<Log> finalList = new List<Log>();

            foreach(KeyValuePair<string, List<Log>> gameLogs in _database.ErrorLogs[username])
            {
                finalList.Union(gameLogs.Value);
            }

            return finalList;
        }

        public List<Log> Get(string username, string gamename)
        {
            return _database.ErrorLogs[username][gamename];
        }

        public List<Log> GetAll()
        {
            // Agarramos los values
            // Por cada value, agarramos los values
            List<Log> finalList = new List<Log>();

            foreach(KeyValuePair<string, Dictionary<string, List<Log>>> userLogs in _database.ErrorLogs)
            {
                foreach(KeyValuePair<string, List<Log>> gameLogs in userLogs.Value)
                {
                    finalList.Union(gameLogs.Value);
                }
            }

            return finalList;
        }
    }
}