using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using DataAccess.Interface;
using Database.Interface;
using Domain;

namespace DataAccess
{
    public class LogDataAccessHelper
    {
        public static List<Log> GetLogsFromDictionary(Dictionary<string, List<Log>> logs)
        {
            List<Log> finalList = new List<Log>();

            foreach(KeyValuePair<string, List<Log>> gameLogs in logs)
            {
                finalList.Union(gameLogs.Value);
            }

            return finalList;
        }
        public static List<Log> GetUserLogsFromConcurrentDictionary(ConcurrentDictionary<string, Dictionary<string, List<Log>>> logs)
        {
            List<Log> finalList = new List<Log>();

            foreach(KeyValuePair<string, Dictionary<string, List<Log>>> userLogs in logs)
            {
                finalList.Union(GetLogsFromDictionary(userLogs.Value));
            }

            return finalList;
        }

        public static List<Log> GetGameLogsFromConcurrentDictionary(ConcurrentDictionary<string, Dictionary<string, List<Log>>> logs, string gamename)
        {
            List<Log> finalList = new List<Log>();

            foreach(KeyValuePair<string, Dictionary<string, List<Log>>> userLogs in logs)
            {
                finalList.Union(GetLogsFromDictionary(userLogs.Value).Where(l => l.Gamename == gamename));
            }

            return finalList;
        }
    }
}